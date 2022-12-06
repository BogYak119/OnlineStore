using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static readonly List<string> roles = new List<string>() { "admin", "customer" };

        public static string SessionToken = "JWTToken";
    }
}
