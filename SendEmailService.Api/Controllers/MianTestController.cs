using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using SendEamilService.Domain;
using SendEamilService.Infrastructure;
using System.Data;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Text;
using static SendEamilService.Domain.Utils.Enums;
using MailKit.Net.Smtp;
using Outlook = Microsoft.Office.Interop.Outlook;
using MailKit.Security;
using System.Net.Mail;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SendEmailService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MianTestController : ControllerBase
    {

        /// <summary>
        /// 数据上下文
        /// </summary>
        private readonly MyDbContext _coreDbContext;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 注入上下文
        /// </summary>
        /// <param name="coreDbContext"></param>
        public MianTestController(MyDbContext coreDbContext, IConfiguration configuration)
        {
            _coreDbContext = coreDbContext;
            _configuration = configuration;
        }
        private string icon = string.Empty;
        private string strHTML = string.Empty;

        #region 控制器接口名称
        /// <summary>
        /// 获取树型节点数据格式
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTreeData")]
        public ContentResult GetTreeData()
        {
            try
            {
                List<myBusinessUnitModel> myNoteFiles = _coreDbContext.Set<myBusinessUnitModel>().OrderBy(a => a.OrderHierarchyCode).ToList();
                //递归解析数据
                List<ProjectUnitModel> treeModelstt = new List<ProjectUnitModel>();

                ParseTree(myNoteFiles, treeModelstt, null);
                string jsonStr = JsonConvert.SerializeObject(treeModelstt);
                return Content(jsonStr, "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ei"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailInformationgModel ei)
        {
            try
            {
                var qurey = from u in _coreDbContext.Set<MessageSettingEmailModel>()
                            where u.UserName == _configuration["MessageUser:UserId"]
                            select u;

                var result = qurey.FirstOrDefault();
                if (result != null)
                {
                    /*发送邮件方法一：*/
                    MailMessage mymail = new MailMessage();
                    mymail.From = new System.Net.Mail.MailAddress(result.UserEmail,"管理员");
                    mymail.To.Add(ei.strRecipient);
                    mymail.CC.Add(ei.strCC);
                    mymail.Bcc.Add(result.UserEmail);
                    mymail.Subject = ei.strTitle;
                    mymail.Body = ei.strContent;
                    mymail.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient();
                    smtpclient.Timeout = 8000;//毫秒
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpclient.Port = 587;
                    smtpclient.Host = "mail.gemdale.com";
                    smtpclient.EnableSsl = true;
                    smtpclient.Credentials = new System.Net.NetworkCredential(result.UserEmail, result.Password);
                    smtpclient.Send(mymail);

                    /*发送邮件方法二：*/
                    // var messageToSend = new MimeMessage
                    //{
                    //    Sender = new MailboxAddress(result.UserName, result.UserEmail),//发件人
                    //    Subject = ei.strTitle,
                    //    Body = new TextPart(TextFormat.Html) { Text = ei.strContent },
                    //};
                    //    messageToSend.To.Add(new MailboxAddress("收件人", ei.strRecipient));
                    //    messageToSend.Cc.Add(new MailboxAddress("抄送人", ei.strCC));
                    //    using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                    //{
                    //    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //    await smtp.ConnectAsync("mail.gemdale.com", 587, SecureSocketOptions.StartTls);
                    //    await smtp.AuthenticateAsync(result.UserEmail, result.Password);
                    //    await smtp.SendAsync(messageToSend);
                    //    await smtp.DisconnectAsync(true);
                    //}
                    return OK(ApiReturnCode.Success, "", null);
                }
                else {
                    return OK(ApiReturnCode.Failed, "验证身份邮箱有误，请检查！", null);
                }

            }
            catch (Exception ex) { }
            return OK(ApiReturnCode.Failed, "", null);
        }

        [HttpPost("GetMessageSettingEmail")]
        public IActionResult GetMessageSettingEmail()
        {
            var qurey = from u in _coreDbContext.Set<MessageSettingEmailModel>()
                        where u.UserName == _configuration["MessageUser:UserId"]
                        select u;
            var result = qurey.FirstOrDefault();
            if (result != null)
            {
                return OK(ApiReturnCode.Success, "", result);
            }
            else
            {
                return OK(ApiReturnCode.Success, "无权限操作", null);
            }
        }

        [HttpPost("UpdateMessage")]
        public IActionResult UpdateMessage(MessageSettingEmail param)
        {
            try
            {
                MessageSettingEmailModel ms = _coreDbContext.Set<MessageSettingEmailModel>().Where(u => u.ID == param.ID).FirstOrDefault();
                if (ms != null)
                {
                    ms.QYEmail = param.QYEmail;
                    ms.UserEmail = param.UserEmail;
                    ms.Password = param.Password;
                    ms.modificationTim = DateTime.Now;
                    _coreDbContext.messageSettingEmail.Update(ms);
                    _coreDbContext.SaveChangesAsync().Wait();
                    return OK(ApiReturnCode.Success, "修改成功！", null);
                }
                else
                {
                    return OK(ApiReturnCode.Failed, "修改失败！", null);
                }
            }
            catch (Exception ex)
            {
                return OK(ApiReturnCode.Failed, ex.Message, null);
            }
            
        }
        [HttpPost("createUser")]
        public IActionResult createUser(ADUserModel param)
        {
            return OK(ApiReturnCode.Success, GetUser(param), null);
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取树型节点数据，递归解析数据；//如果暴漏在外面的接口就不能包含又多个list类型的参数，可以把他们封装到一个参数类型上再做传参
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="treeModels"></param>
        /// <param name="parentId"></param>
        private void ParseTree(List<myBusinessUnitModel> departments, List<ProjectUnitModel> treeModels, Guid? parentId)
        {

            // 找当前层级下级(如果parentId == null那就是第一级)
            List<myBusinessUnitModel> result = departments.Where(a => a.ParentGUID == parentId ).ToList();
            foreach (myBusinessUnitModel item in result)
            {
                if (item.Level == 0)
                {
                    icon = "home-2";
                }
                else if (item.Level == 1)
                {
                    icon = "table";
                }
                else if (item.Level == 2)
                {
                    icon = "twitter";
                }
                ProjectUnitModel treeModel = new ProjectUnitModel();
                treeModel.Id = item.BUGUID;
                treeModel.text = item.BUName;
                treeModel.icon = icon;
                //  递归
                ParseTree(departments, treeModel.children, treeModel.Id);
                treeModels.Add(treeModel);
            }
        }

        /// <summary>
        /// 查询域用户与信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("GetADinformation")]
        public IActionResult GetADinformation(UserInformationModel param)
        {
            if (!string.IsNullOrEmpty(param.UserName) || !string.IsNullOrEmpty(param.UserId))
            {
                string conAD = String.Empty;
                if (!string.IsNullOrEmpty(param.UserName.Trim()))
                {
                    conAD = "(&(objectClass=user)(objectcategory=person)(Name=*" + param.UserName.Trim() + "))";
                }
                else
                {
                    conAD = "(&(objectClass=user)(objectcategory=person)(sAMAccountName=" + param.UserId.Trim() + "))";
                }
                DataTable dic = new DataTable();
                //创建表的列
                dic.Columns.Add("depmGUID", typeof(System.String));
                dic.Columns.Add("userGUID", typeof(System.String));
                dic.Columns.Add("userName", typeof(System.String));
                dic.Columns.Add("userID", typeof(System.String));
                dic.Columns.Add("department", typeof(System.String));
                dic.Columns.Add("userStatus", typeof(System.String));
                dic.Columns.Add("title", typeof(System.String));
                dic.Columns.Add("Email", typeof(System.String));
                DirectoryEntry entry1 = new DirectoryEntry("LDAP://OU=金地集团,DC=gemdale,DC=com");
                try
                {
                    DirectorySearcher search = new DirectorySearcher(entry1);
                    search.Filter = conAD;
                    SearchResult result = search.FindOne(); //查询一条的写法
                    if (result != null)
                    {

                        foreach (var item in search.FindAll())
                        {
                            DataRow dr = dic.NewRow();
                            SearchResult searchResult = (SearchResult)item;
                            var objOneDirEnt = searchResult.GetDirectoryEntry();
                            string userId = objOneDirEnt.Properties["sAMAccountName"]?.Value.ToString();
                            if (userId != null && userId.Length > 0)
                            {
                                //部门GUID
                                dr["depmGUID"] = objOneDirEnt.Parent.Guid.ToString();
                                //用户GUID
                                dr["userGUID"] = objOneDirEnt.Guid.ToString();
                                if (objOneDirEnt != null)
                                {
                                    foreach (var type in objOneDirEnt.Properties.PropertyNames)
                                    {
                                        string strType = (string)type;
                                        if (strType.CompareTo("displayName") == 0)
                                        {//用户名
                                            dr["userName"] = objOneDirEnt.Properties["displayname"]?[0].ToString();
                                        }
                                        else if (strType.CompareTo("sAMAccountName") == 0)
                                        {//用户账号
                                            dr["userID"] = objOneDirEnt.Properties["sAMAccountName"]?[0].ToString();
                                        }
                                        else if (strType.CompareTo("department") == 0)
                                        {//部门 objOneDirEnt.Properties["department"]?[0].ToString();
                                            dr["department"] = objOneDirEnt.Parent.Name.Substring(3);
                                        }
                                        else if (strType.CompareTo("userAccountControl") == 0)
                                        {//账号状态
                                            dr["userStatus"] = objOneDirEnt.Properties["userAccountControl"]?[0].ToString();
                                        }
                                        else if (strType.CompareTo("title") == 0)
                                        {//职务
                                            dr["title"] = objOneDirEnt.Properties["title"]?[0].ToString();
                                        }
                                        else if (strType.CompareTo("mail") == 0)
                                        {
                                            dr["Email"] = objOneDirEnt.Properties["mail"]?[0].ToString();
                                        }
                                    }
                                }
                            }
                            dic.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        return OK(ApiReturnCode.Failed, "无此用户AD信息！", null);
                    }
                    return OK(ApiReturnCode.Success, "查询完成", JsonConvert.SerializeObject(dic));
                }
                catch (Exception ex)
                {
                    return OK(ApiReturnCode.Failed, ex.Message, null);
                }
            }
            else
            {
                return OK(ApiReturnCode.Failed, "无用户ID和用户姓名！", null);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult OK(ApiReturnCode code, string msg = null, object data = null)
        {
            var result = new
            {
                code,
                msg,
                data
            };
            return Ok(result);
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetUser(ADUserModel param)
        {
            
            string result3 = string.Empty;
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes("Aa12345678!!"));
                var strResult = BitConverter.ToString(result);
                result3 = strResult.Replace("-", "").ToLower();
            }
            if (!param.createUser && !param.systype.Contains("crm") )
            {
                
                var qurey = from u in _coreDbContext.Set<UserModel>()
                            where u.UserCode == param.ADId
                            select new { u.UserGUID, u.UserCode};
                var result = qurey.FirstOrDefault();
                if (result == null)
                {
                    //用户表
                    Guid uGUID = Guid.NewGuid();
                    UserModel user = new UserModel();
                    user.ADAccount = param.ADGUID.ToString().Trim();
                    user.BUGUID = param.gsGUID;
                    user.Email = param.email;
                    user.IsAdmin = 0;
                    user.IsDisabeld = param.status;   // 1：表示禁用账号  0：表示启用账号
                    user.IsFirstLogin = 1;
                    user.IsModelingUser = 0;
                    user.IsOldUser = 0;
                    user.IsUserChangePWD = 0;
                    user.Password = result3;
                    user.PasswordModifyTime = DateTime.Now;
                    user.UserCode = param.ADId.ToString().Trim();
                    user.UserKind = param.systype;
                    user.UserName = param.ADName.ToString().Trim();
                    user.CreatedGUID = new Guid("06219f84-97e7-4c4c-9495-03f8bbdb7ac7");
                    user.CreatedName = "卢泽伟";
                    user.CreatedTime = DateTime.Now;
                    user.ModifiedGUID = Guid.NewGuid();
                    user.ModifiedName = "卢泽伟";
                    user.ModifiedTime = DateTime.Now;
                    user.UserGUID = uGUID;
                    _coreDbContext.myUser.Add(user);

                    ////用户组织映射表
                    UnitMapping map = new UnitMapping();
                    map.myUserBusinessUnitMappingId = Guid.NewGuid();
                    map.UserId = uGUID;
                    map.BUGUID = param.gsGUID;
                    map.ModifiedName = "卢泽伟";
                    map.CreatedGUID = new Guid("06219f84-97e7-4c4c-9495-03f8bbdb7ac7");
                    map.CreatedName = "卢泽伟";
                    map.CreatedTime = DateTime.Now;
                    map.ModifiedGUID = Guid.NewGuid();
                    map.ModifiedTime = DateTime.Now;
                    _coreDbContext.myUserBusinessUnitMapping.Add(map);
                    _coreDbContext.SaveChangesAsync().Wait();


                    return "用户创建成功！";
                }
                else
                {
                    return "用户已存在！";
                }
            }
            else
            {
                return "无需创建新用户！";
            }
        }

        #endregion

       
      
        [HttpGet]
        [Route("GetMyBusinesses")]
        public IActionResult GetMyBusinesses()
        {
           

            List<myBusinessUnitModel> myNoteFiles = _coreDbContext.Set<myBusinessUnitModel>().OrderBy(a=>a.OrderHierarchyCode).ToList();

            var query = from bm in _coreDbContext.Set<myBusinessUnitModel>().ToList()
                        join gs in _coreDbContext.Set<myBusinessUnitModel>().ToList() on bm.ParentGUID equals gs.BUGUID
                        join qy in _coreDbContext.Set<myBusinessUnitModel>().ToList() on gs.ParentGUID equals qy.BUGUID
                        join jt in _coreDbContext.Set<myBusinessUnitModel>().ToList() on qy.ParentGUID equals jt.BUGUID
                        where jt.BUName == "金地集团" && qy.BUName != "测试区域" && qy.BUName != "金地商置公司"
                        select new { bmBUName = bm.BUName, gsBUName = gs.BUName, qyBUName = qy.BUName, jtBUName = jt.BUName, bmBUGUID = bm.BUGUID, gsBUGUID = gs.BUGUID, qyBUGUID = qy.BUGUID, jtBUGUID = jt.BUGUID };

            //var test = query.ToList();

            //List<myBusinessUnitModel> cc = _coreDbContext.myBusinessUnit.ToList();



            return OK(ApiReturnCode.Success, "", query);
        }

     

    }
}

