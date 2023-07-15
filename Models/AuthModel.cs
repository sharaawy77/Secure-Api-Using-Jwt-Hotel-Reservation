namespace SecureApi.Models
{
    public class AuthModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsAuth { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Expireson { get; set; }




    }
}
