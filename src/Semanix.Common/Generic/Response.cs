using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Common.Generic
{
    public class Response<T>
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public T Data { get; set; }
    }
}
