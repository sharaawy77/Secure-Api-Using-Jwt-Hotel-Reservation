using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Secure_Api_Jwt.Data;
using Secure_Api_Jwt.Services;
using SecureApi.Helper;
using SecureApi.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secure_Api_Jwt.Models
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly JWT _jwt;
        public AuthService(RoleManager<IdentityRole> roleManager,ApplicationDbContext context, IOptions<JWT> jwt,UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.context = context;
            this.userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await userManager.FindByEmailAsync(model.Email)!=null)
            {
                return new AuthModel()
                {
                    Message = "Email Already Exist",
                    
                };
            }
            var user = new IdentityUser()
            {
                UserName=model.UserName,
                Email=model.Email
            };
            var Result=await userManager.CreateAsync(user,model.Password);
            if (!Result.Succeeded)
            {
                var error = string.Empty;
                foreach (var er in Result.Errors)
                {
                    error += $"{er.Description}";
                }
                return new AuthModel() { Message = error };
            }
            var jwtToken = await CreateJwtToken(user);
            await userManager.AddToRoleAsync(user, "User");
            return new AuthModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                IsAuth = true,
                Expireson = jwtToken.ValidTo,
                Roles = new List<string>() { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)

            };
        }
        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var user = new EmailAddressAttribute().IsValid(model.Email) ? userManager.FindByEmailAsync(model.Email).Result : userManager.FindByNameAsync(model.Email).Result;
            if (user==null|| !await userManager.CheckPasswordAsync(user, model.PassWord))
            {
                return new AuthModel() { Message = "'UserName & Email' OR Password InCorrect" };
            }

            else
            {
                var jwtToken = await CreateJwtToken(user);
                var roles = userManager.GetRolesAsync(user).Result.ToList();
                return new AuthModel()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    IsAuth = true,
                    Expireson = jwtToken.ValidTo,
                    Roles = roles,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
                };

            }
            
        }
        public async Task<string>  AddToRoleAsync(AddToRoleModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user ==null||!await roleManager.RoleExistsAsync(model.RoleName))
            {
                return "Error in UserId Or Role doesnt Exist";
            }
            if (await userManager.IsInRoleAsync(user,model.RoleName))
            {
                return "User Already in this Role";
            }
            else
            {
                var result=await userManager.AddToRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    return "Error";
                }
                return string.Empty;

            }
        }
        private async Task<JwtSecurityToken> CreateJwtToken(IdentityUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


    }
}
