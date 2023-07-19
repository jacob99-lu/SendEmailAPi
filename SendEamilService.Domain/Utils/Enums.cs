using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Domain.Utils
{
    public class Enums
    {
        public enum ApiReturnCode
        {
            Success = 0,
            Failed = 1,
            Exception = 10001
        }
    }
}
