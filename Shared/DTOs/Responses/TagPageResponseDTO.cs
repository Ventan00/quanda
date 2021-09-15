using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class TagsPageResponseDTO
    {
        public List<TagResponseDTO> Tags { get; set; }
        public int TotalAmountOfTags { get; set; }
    }
}
