using Quanda.Shared;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Quanda.Server.Extensions
{
    public static class CustomExtensionMethods
    {
        public static List<TagResponseDTO> GetTruncatedTags(IEnumerable<Tag> tags, int page)
        {
            return tags.Skip(page * Config.TAGS_PAGE_SIZE).Take((page + 1) * Config.TAGS_PAGE_SIZE)
                .Select(tag => new TagResponseDTO
                {
                    IdTag = tag.IdTag,
                    Name = tag.Name,
                    Description = tag.Description,
                    AmountOfQuestions = tag.QuestionTags.Count
                }).ToList();
        }
    }
}