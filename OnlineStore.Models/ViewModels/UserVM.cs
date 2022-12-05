using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
