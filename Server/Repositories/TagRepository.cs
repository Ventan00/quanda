using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Extensions;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za tagi
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        ///     Zwraca tagi z podanej strony.
        /// </summary>
        /// <param name="page">Aktualna strona przeglądanych tagów.</param>
        /// <param name="sortOption">Rodzaj sortowania tagów.</param>
        /// <returns>TagsPageResponseDTO</returns>
        Task<TagsPageResponseDTO> GetTagsAsync(int page, SortTagsEnum sortOption);

        /// <summary>
        ///     Zwraca subtagi z podanej strony.
        /// </summary>
        /// <param name="idMainTag">Id nadrzędnego tagu.</param>
        /// <param name="page">Aktualna strona przeglądanych subtagów.</param>
        /// <param name="sortOption">Rodzaj sortowania tagów.</param>
        /// <returns>SubTagsPageResponseDTO</returns>
        Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page, SortTagsEnum sortOption);

        /// <summary>
        ///     Zwraca listę tagów danego pytania
        /// </summary>
        /// <param name="idQuestion">ID pytania w BD</param>
        /// <returns>List(TagResponseDTO)</returns>
        Task<List<TagResponseDTO>> GetTagsOfQuestionAsync(int idQuestion);

        /// <summary>
        ///     Uaktualnia informacje o tagu
        /// </summary>
        /// <param name="tag">Obiekt DTO opisujący tag</param>
        /// <param name="idTag">Id tagu w BD</param>
        /// <returns>TagResultEnum</returns>
        Task<TagResultEnum> UpdateTagAsync(UpdateTagDTO tag, int idTag);

        /// <summary>
        ///     Dodaję tag do BD
        /// </summary>
        /// <param name="category">Obiekt DTO opisujący tag</param>
        /// <returns>TagResultEnum</returns>
        Task<TagResultEnum> AddTagAsync(AddTagDTO category);

        /// <summary>
        ///     Usuwa tag z BD
        /// </summary>
        /// <param name="idTag">Id tagu w BD</param>
        /// <returns>TagResultEnum</returns>
        Task<TagResultEnum> DeleteTagAsync(int idTag);
    }

    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TagsPageResponseDTO> GetTagsAsync(int page, SortTagsEnum sortOption)
        {
            List<TagResponseDTO> tags = null;
            switch (sortOption)
            {
                case SortTagsEnum.Popular:
                    tags = CustomExtensionMethods.GetTruncatedTags(_context.Tags.OrderByDescending(tag => tag.QuestionTags.Count), page);
                    break;
                case SortTagsEnum.Name:
                    tags = CustomExtensionMethods.GetTruncatedTags(_context.Tags.OrderBy(tag => tag.Name), page);
                    break;
            }

            return new TagsPageResponseDTO
            {
                Tags = tags,
                TotalAmountOfTags = _context.Tags.Count()
            };
        }

        public async Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page, SortTagsEnum sortOption)
        {
            List<TagResponseDTO> subTags = null;
            switch (sortOption)
            {
                case SortTagsEnum.Popular:
                    subTags = CustomExtensionMethods.GetTruncatedTags(_context.Tags.Where(t => t.IdMainTag == idMainTag).OrderByDescending(tag => tag.QuestionTags.Count), page);
                    break;
                case SortTagsEnum.Name:
                    subTags = CustomExtensionMethods.GetTruncatedTags(_context.Tags.Where(t => t.IdMainTag == idMainTag).OrderBy(tag => tag.Name), page);
                    break;
            }

            var mainTag = await _context.Tags.SingleOrDefaultAsync(t => t.IdTag == idMainTag);

            return new SubTagsPageResponseDTO
            {
                IdMainTag = idMainTag,
                NameMainTag = mainTag.Name,
                SubTags = subTags,
                TotalAmountOfSubTags = _context.Tags.Count(t => t.IdMainTag == idMainTag)
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