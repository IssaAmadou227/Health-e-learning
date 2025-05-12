using FutureDocteur.API.DataBase.Repository.Contract;
using FutureDocteur.API.Models;
using FutureDocteur.API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FutureDocteur.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IBaseRepository<Store> _storeRepository;
        private readonly ResponseDto _responseDto;

        public StoreController(IBaseRepository<Store> storeRepository)
        {
            _storeRepository = storeRepository;
            _responseDto = new ResponseDto();
        }

        [HttpPost("create")]
        public async Task<ResponseDto> CreateStore([FromBody] CreateStoreDto model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Phone))
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Email et téléphone sont requis.";
                    return _responseDto;
                }

                var store = new Store
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    LogoUrl = model.LogoUrl,
                    CoverPhotoUrl = model.PhotoCovert,
                    OwnerId = model.OwnerId,
                    CreatedDate = DateTime.UtcNow
                };

                await _storeRepository.AddAsync(store);
                await _storeRepository.SaveAsync();

                _responseDto.IsSucces = true;
                _responseDto.Message = "Boutique créée avec succès.";
                _responseDto.Object = store;
            }
            catch (Exception ex)
            {
                _responseDto.IsSucces = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPut("edit/{id:guid}")]
        public async Task<ResponseDto> EditStore(Guid id, [FromBody] CreateStoreDto model)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Boutique introuvable.";
                    return _responseDto;
                }

                store.Email = model.Email;
                store.Phone = model.Phone;
                store.Address = model.Address;
                store.LogoUrl = model.LogoUrl;
                store.CoverPhotoUrl = model.PhotoCovert;

                _storeRepository.Update(store);
                await _storeRepository.SaveAsync();

                _responseDto.IsSucces = true;
                _responseDto.Message = "Boutique mise à jour avec succès.";
                _responseDto.Object = store;
            }
            catch (Exception ex)
            {
                _responseDto.IsSucces = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet("{id:guid}")]
        public async Task<ResponseDto> GetStoreById(Guid id)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Boutique introuvable.";
                }
                else
                {
                    _responseDto.IsSucces = true;
                    _responseDto.Object = store;
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSucces = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet("all")]
        public async Task<ResponseDto> GetAllStores()
        {
            try
            {
                var stores = await _storeRepository.GetAllAsync();
                _responseDto.IsSucces = true;
                _responseDto.Object = stores;
            }
            catch (Exception ex)
            {
                _responseDto.IsSucces = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ResponseDto> DeleteStore(Guid id)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Boutique introuvable.";
                    return _responseDto;
                }

                _storeRepository.Delete(store);
                await _storeRepository.SaveAsync();

                _responseDto.IsSucces = true;
                _responseDto.Message = "Boutique supprimée avec succès.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSucces = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }
    }
}
