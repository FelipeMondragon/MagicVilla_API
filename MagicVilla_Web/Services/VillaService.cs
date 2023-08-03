using MagicVilla_Utility;
using MagicVilla_Web.DTOs;
using MagicVilla_Web.IServices;
using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string _villaUrl;

        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory) 
        {
            _clientFactory = clientFactory;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                URL = _villaUrl + "/api/villaAPI",
                Data = dto
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                URL = _villaUrl + "/api/villaAPI/" + id,
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                URL = _villaUrl + "/api/villaAPI",
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest() 
            { 
                ApiType = SD.ApiType.GET, 
                URL = _villaUrl + "/api/villaAPI/" + id 
            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest() 
            { 
                ApiType = SD.ApiType.PUT, 
                URL = _villaUrl + "/api/villaAPI/" + dto.Id,
                Data = dto
            });
        }
    }
}
