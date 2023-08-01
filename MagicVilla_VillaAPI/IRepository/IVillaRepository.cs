using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.IRepository
{
    public interface IVillaRepository : IGenericRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}
