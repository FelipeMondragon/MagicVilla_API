using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.IRepository
{
    public interface IVillaNumberRepository : IGenericRepository<VillaNumber>
    {
        public Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}
