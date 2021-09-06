using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.DTOs.Responses
{
    public class Top3UserResponseDTO
    {
        public string Nickname { get; set; }
        public int IdUser { get; set; }
        public string Avatar { get; set; }
        public int Points { get; set; }
    }
}
