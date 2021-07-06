using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface ICategoriesReposiotry
    {
        public Task<List<CategoriesResponseDTO>> GetCategories();
    }
}
