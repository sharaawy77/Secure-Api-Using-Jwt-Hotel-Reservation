using Microsoft.Win32;
using NuGet.Protocol.Plugins;
using Secure_Api_Jwt.Models;
using SecureApi.Models;

namespace Secure_Api_Jwt.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(RegisterModel model);
        public Task<AuthModel> LoginAsync(LoginModel model);
        public Task< string> AddToRoleAsync(AddToRoleModel model);

    }
}
