using FutureDocteur.API.DataBase.Repository.Contract;
using FutureDocteur.API.Models;
using FutureDocteur.API.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutureDocteur.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmail _emailSender;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBaseRepository<ApplicationUser> _baseRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager,
            SignInManager<ApplicationUser> signInManager, IEmail emailSender, IBaseRepository<ApplicationUser> baseRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _baseRepository = baseRepository;
        }

        // ✅ Enregistrement d’un utilisateur
        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto>> Register([FromForm] RegisterUserDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest(new ResponseDto
                {
                    IsSucces = false,
                    Message = "Un utilisateur avec cet e-mail existe déjà."
                });
            }

            var code = new Random().Next(100000, 999999).ToString(); // Code 6 chiffres

            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = false,
                EmailVerificationCode = code,
                CodeExpiration = DateTime.UtcNow.AddHours(2)
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseDto
                {
                    IsSucces = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            if (!string.IsNullOrEmpty(dto.RoleName))
            {
                if (!await _roleManager.RoleExistsAsync(dto.RoleName))
                {
                    await _roleManager.CreateAsync(new Role { Name = dto.RoleName });
                }

                await _userManager.AddToRoleAsync(newUser, dto.RoleName);
            }

            var message = $"Bonjour {dto.FirstName}, votre code de vérification est : <b>{code}</b>. Il expire dans 2 heures.";
            await _emailSender.SendEmailAsync(dto.Email, "Code de vérification", message);

            return Ok(new ResponseDto
            {
                IsSucces = true,
                Message = "Inscription réussie. Un code de vérification vous a été envoyé par email."
            });
        }
        [HttpPost("verify-email-code")]
        public async Task<ActionResult<ResponseDto>> VerifyCode([FromBody] VerifyCodeDto dto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.EmailVerificationCode == dto.Code);

            if (user == null)
            {
                return NotFound(new ResponseDto { IsSucces = false, Message = "Code invalide ou utilisateur introuvable." });
            }

            if (user.EmailConfirmed)
            {
                return BadRequest(new ResponseDto { IsSucces = false, Message = "Email déjà confirmé." });
            }

            if (user.CodeExpiration < DateTime.UtcNow)
            {
                return BadRequest(new ResponseDto { IsSucces = false, Message = "Code expiré." });
            }

            user.EmailConfirmed = true;
            user.EmailVerificationCode = null;
            user.CodeExpiration = null;

            await _userManager.UpdateAsync(user);

            return Ok(new ResponseDto { IsSucces = true, Message = "Email confirmé avec succès." });
        }




        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto>> Login([FromForm] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized(new ResponseDto
                {
                    IsSucces = false,
                    Message = "Utilisateur introuvable."
                });
            }
            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ResponseDto
                {
                    IsSucces = false,
                    Message = "Mot de passe incorrect."
                });
            }

            return Ok(new ResponseDto
            {
                IsSucces = true,
                Message = "Connexion réussie."
            });
        }


        [HttpPost("logout")]
        public async Task<ActionResult<ResponseDto>> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ResponseDto
            {
                IsSucces = true,
                Message = "Déconnexion réussie."
            });
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ResponseDto>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound(new ResponseDto { IsSucces = false, Message = "Utilisateur introuvable." });

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new ResponseDto
                {
                    IsSucces = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                });

            return Ok(new ResponseDto
            {
                IsSucces = true,
                Message = "Mot de passe modifié avec succès."
            });
        }

        [HttpPost("assign-role")]
        public async Task<ActionResult<ResponseDto>> AssignRole([FromBody] AssignRoleDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound(new ResponseDto { IsSucces = false, Message = "Utilisateur introuvable." });

            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(new Role { Name = dto.Role });
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new ResponseDto
            {
                IsSucces = true,
                Message = $"Rôle {dto.Role} affecté à l'utilisateur."
            });
        }
    }
}
