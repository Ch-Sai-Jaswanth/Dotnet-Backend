using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BikeDealersProject.AuthModels;
using BikeDealersProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
namespace BikeDealersProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<(int, string)> RegisterAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return (0, "User already exists");

            IdentityUser user = new()
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return (0, "User creation failed");

            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));

            await _userManager.AddToRoleAsync(user, model.Role);

            return (1, "User created successfully");
        }

        public async Task<(int, string, IEnumerable<string>)> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Invalid credentials", null);

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(6),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                    SecurityAlgorithms.HmacSha256
                )
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (1, tokenString, userRoles);
        }
    }
}
