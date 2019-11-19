using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSample
{
    public class ResponseDto
    {
        public StatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public virtual decimal Data { get; set; }
    }

    public enum StatusCode
    {
        Fail = 0,
        Success = 1
    }
}
