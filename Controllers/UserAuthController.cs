using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

[Route("api")]
[ApiController]
public class UserAuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    public UserAuthController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Users model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Eğer model geçerliyse, parolayı hash'leyip kaydetme işlemi yapar
        model.Password = HashPasswordMD5(model.Password);
        _context.Users.Add(model);
        await _context.SaveChangesAsync();

        return Ok();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username || x.Email == model.Username);
        if (user == null)
            return NotFound("User not found.");
        if (user.Password != HashPasswordMD5(model.Password))
            return Unauthorized("Invalid password.");
        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    private string GenerateJwtToken(Users user)
    {
        var claims = new[]
       {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("burak", user.id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"],
               audience: _configuration["Jwt:Audience"],
               claims: claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static string HashPasswordMD5(string password)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Hash'i hex formatında geri döndürmek için StringBuilder kullanıyoruz
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2")); // Hex formatında temsil
            }

            return sb.ToString();
        }
    }
}

