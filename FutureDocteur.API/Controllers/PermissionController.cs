using FutureDocteur.API.DataBase.Repository.Contract;
using FutureDocteur.API.Models;
using FutureDocteur.API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FutureDocteur.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IBaseRepository<Permission> _permissionRepository;
        private readonly ResponseDto _response;

        public PermissionController(IBaseRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
            _response = new ResponseDto();
        }

        [HttpPost("create")]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreatePermissionDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                _response.IsSucces = false;
                _response.Message = "Le nom de la permission est requis.";
                return BadRequest(_response);
            }

            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description
            };

            await _permissionRepository.AddAsync(permission);
            await _permissionRepository.SaveAsync();

            _response.IsSucces = true;
            _response.Message = "Permission créée avec succès.";
            return Ok(_response);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAll()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return Ok(permissions);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Permission>> GetById(Guid id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
                return NotFound("Permission introuvable.");

            return Ok(permission);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ResponseDto>> Update([FromBody] UpdatePermissionDto model)
        {
            var permission = await _permissionRepository.GetByIdAsync(model.Id);
            if (permission == null)
            {
                _response.IsSucces = false;
                _response.Message = "Permission introuvable.";
                return NotFound(_response);
            }

            permission.Name = model.Name;
            permission.Description = model.Description;

            _permissionRepository.Update(permission);
            await _permissionRepository.SaveAsync();

            _response.IsSucces = true;
            _response.Message = "Permission mise à jour avec succès.";
            return Ok(_response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
            {
                _response.IsSucces = false;
                _response.Message = "Permission introuvable.";
                return NotFound(_response);
            }

            _permissionRepository.Delete(permission);
            await _permissionRepository.SaveAsync();

            _response.IsSucces = true;
            _response.Message = "Permission supprimée avec succès.";
            return Ok(_response);
        }
    }
}
