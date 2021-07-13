using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.DTOs.Responses
{
    public class CategoriesResponseDTO
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public int? IdMainCategory { get; set; }
    }
}
