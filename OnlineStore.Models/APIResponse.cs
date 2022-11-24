using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool isSuccess { get; set; } = true;

        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
