using FutureDocteur.API.DataBase.Repository.Contract;
using FutureDocteur.API.Models;
using FutureDocteur.API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FutureDocteur.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly ResponseDto _responseDto;
        private readonly IBaseRepository<RolePermission> _rolePermissionRepository;
        public RoleController(IBaseRepository<Role> roleRepository, IBaseRepository<RolePermission> rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _responseDto = new ResponseDto();
            _rolePermissionRepository = rolePermissionRepository;
        }

        [HttpPost("create")]
        public async Task<ResponseDto> CreateRole([FromBody] CreateRoleDto model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Le nom du rôle est requis.";
                }
                else
                {
                    var newRole = new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description
                    };

                    await _roleRepository.AddAsync(newRole);
                    await _roleRepository.SaveAsync();

                    _responseDto.IsSucces = true;
                    _responseDto.Message = "Rôle créé avec succès";
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSucces = false;
            }

            return _responseDto;
        }

        [HttpPut("edit/{id:guid}")]
        public async Task<ResponseDto> EditRole(Guid id, [FromBody] CreateRoleDto model)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Rôle introuvable.";
                    return _responseDto;
                }

                role.Name = model.Name;
                role.NormalizedName = model.Name.ToUpper();
                role.Description = model.Description;

                _roleRepository.Update(role);
                await _roleRepository.SaveAsync();

                _responseDto.IsSucces = true;
                _responseDto.Message = "Rôle mis à jour avec succès.";
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSucces = false;
            }

            return _responseDto;
        }
        [HttpPost("assign-permissions")]
        public async Task<ResponseDto> AssignPermissionsToRole([FromBody] AssignPermissionsDto model)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(model.RoleId);
                if (role == null)
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Rôle introuvable.";
                    return _responseDto;
                }

                var existingPermissions = role.RolePermissions?.ToList() ?? new List<RolePermission>();

                foreach (var old in existingPermissions)
                {
                    _rolePermissionRepository.Delete(old);
                }

                foreach (var permissionId in model.PermissionIds)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = model.RoleId,
                        PermissionId = permissionId
                    };
                    await _rolePermissionRepository.AddAsync(rolePermission);
                }

                await _rolePermissionRepository.SaveAsync();

                _responseDto.IsSucces = true;
                _responseDto.Message = "Permissions assignées avec succès.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSucces = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }


        [HttpDelete("{id:guid}")]
        public async Task<ResponseDto> DeleteRole(Guid id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    _responseDto.IsSucces = false;
                    _responseDto.Message = "Rôle introuvable.";
                    return _responseDto;
                }

                _roleRepository.Delete(role);
                await _roleRepository.SaveAsync();

                _responseDto.IsSucces = true;
                _responseDto.Message = "Rôle supprimé avec succès.";
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSucces = false;
            }

            return _responseDto;
        }
    }
}
