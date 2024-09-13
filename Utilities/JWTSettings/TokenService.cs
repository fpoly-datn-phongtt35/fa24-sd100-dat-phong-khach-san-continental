using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Utilities.JWTSettings
{
    public class TokenService
    {
        private readonly string _privateKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _expirationMinutes;

        public TokenService(string privateKey, string jwtIssuer, string jwtAudience, int expirationMinutes)
        {
            _privateKey = privateKey;
            _jwtIssuer = jwtIssuer;
            _jwtAudience = jwtAudience;
            _expirationMinutes = expirationMinutes;
        }
        public string CreateToken<T>(T obj, IDictionary<string, string> claims)
        {
            var claimsIdentity = new ClaimsIdentity();

            // Chuyển đối tượng thành JSON và thêm vào claims
            var jsonObj = JsonConvert.SerializeObject(obj);
            claimsIdentity.AddClaim(new Claim("Object", jsonObj));

            // Thêm các claims từ dictionary
            foreach (var claim in claims)
            {
                claimsIdentity.AddClaim(new Claim(claim.Key, claim.Value));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_privateKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
                SigningCredentials = creds,
                Issuer = _jwtIssuer,
                Audience = _jwtAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_privateKey);

            ClaimsPrincipal principal;

            try
            {
                principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_privateKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtIssuer,
                    ValidAudience = _jwtAudience
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu token không hợp lệ
                throw new SecurityTokenException("Invalid token", ex);
            }

            return principal;
        }
    }
}
//-----------------------------------------------------------------------------------
//Usage 
//var user = new { Username = "example", Role = "Admin" };
//var claims = new Dictionary<string, string>
//{
//    { "Email", "user@example.com" },
//    { "Department", "IT" }
//};

//string token = tokenService.GenerateToken(user, claims);