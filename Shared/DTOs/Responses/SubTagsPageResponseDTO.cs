using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class SubTagsPageResponseDTO
    {
        public TagResponseDTO MainTag { get; set; }
        public List<TagResponseDTO> SubTags { get; set; }
        public int TotalAmountOfSubTags { get; set; }
    }
}
