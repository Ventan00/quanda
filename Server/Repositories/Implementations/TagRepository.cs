using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Implementations
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TagsPageResponseDTO> GetTagsAsync(int page)
        {
            var totalAmountOfTags = _context.Tags.Count();
            var tags = await _context.Tags
                .Select(tag => new TagResponseDTO
                {
                    IdTag = tag.IdTag,
                    IdMainTag = tag.IdMainTag,
                    Name = tag.Name,
                    Description = tag.Description,
                    AmountOfQuestions = tag.QuestionTags.Count
                }).Skip(page * Config.TAGS_PAGE_SIZE).Take((page + 1) * Config.TAGS_PAGE_SIZE)
                .ToListAsync();

            return new TagsPageResponseDTO
            {
                Tags = tags,
                TotalAmountOfTags = totalAmountOfTags
            };
        }

        public async Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page)
        {
            var subTags = await _context.Tags.Where(t => t.IdMainTag == idMainTag)
                .Select(tag => new TagResponseDTO
                {
                    IdTag = tag.IdTag,
                    Name = tag.Name,
                    Description = tag.Description,
                    AmountOfQuestions = tag.QuestionTags.Count
                }).ToListAsync();

            var mainTag = await _context.Tags.SingleOrDefaultAsync(t => t.IdMainTag == idMainTag);

            return new SubTagsPageResponseDTO
            {
                IdMainTag = idMainTag,
                NameMainTag = mainTag.Name,
                SubTags = subTags.Skip(page * Config.TAGS_PAGE_SIZE).Take((page + 1) * Config.TAGS_PAGE_SIZE).ToList(),
                TotalAmountOfSubTags = subTags.Count
            };
        }

        public async Task<List<TagResponseDTO>> GetTagsOfQuestionAsync(int idTag)
        {
            return await _context.QuestionTags
                .Include(qt => qt.IdTagNavigation)
                .Where(qt => qt.IdQuestion == idTag)
                .Select(qt => qt.IdTagNavigation)
                .Select(tag => new TagResponseDTO
                {
                    IdTag = tag.IdTag,
                    IdMainTag = tag.IdMainTag,
                    Name = tag.Name
                })
                .ToListAsync();
        }

        public async Task<TagResultEnum> UpdateTagAsync(UpdateTagDTO tag, int idTag)
        {
            var Tag = await _context.Tags.Where(tag => tag.IdTag == idTag).SingleAsync();
            if (Tag == null)
                return TagResultEnum.TAG_NOT_FOUND;
            Tag.IdMainTag = tag.IdMainTag;
            Tag.Name = tag.Name;
            return await _context.SaveChangesAsync() == 1
                ? TagResultEnum.TAG_UPDATED
                : TagResultEnum.TAG_DATABASE_ERROR;
        }

        public async Task<TagResultEnum> AddTagAsync(AddTagDTO tag)
        {
            var Tag = new Tag
            {
                IdMainTag = tag.IdMainTag,
                Name = tag.Name
            };
            await _context.AddAsync(Tag);
            return await _context.SaveChangesAsync() == 1
                ? TagResultEnum.TAG_CREATED
                : TagResultEnum.TAG_DATABASE_ERROR;
        }

        public async Task<TagResultEnum> DeleteTagAsync(int idTag)
        {
            var Tag = await _context.Tags.Where(cat => cat.IdTag == idTag).SingleAsync();
            if (Tag == null)
                return TagResultEnum.TAG_NOT_FOUND;
            _context.Remove(Tag);
            return await _context.SaveChangesAsync() == 1
                ? TagResultEnum.TAG_DELETED
                : TagResultEnum.TAG_DATABASE_ERROR;
        }

    }
}