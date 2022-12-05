using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models.DTO
{
    public class RegistrationResponseDTO
    {
        public UserDTO userDTO { get; set; }
        public List<string>? Errors { get; set; }
    }
}
