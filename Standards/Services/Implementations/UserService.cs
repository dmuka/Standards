using Microsoft.IdentityModel.Tokens;
using Standards.Models.Persons;
using Standards.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Standards.Services.Implementations
{
    public class UserService //: IUserService
    {
        //private readonly IConfiguration _configuration;

        //public UserService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //public Person Authenticate(string username, string password)
        //{
        //    var user = _users.SingleOrDefault(user => user.Username == username && user.Password == password);

        //    if (user == null) return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configuration["Secrets:UserSecret"]);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Id.ToString()),
        //            new Claim(ClaimTypes.Role, user.Role)
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    user.Token = tokenHandler.WriteToken(token);

        //    // remove password before returning
        //    user.Password = null;

        //    return user;
        //}

        //public IEnumerable<User> GetAll()
        //{
        //    // return users without passwords
        //    return _users.Select(user => {
        //        user.Password = null;
        //        return user;
        //    });
        //}

        //public User GetById(int id)
        //{
        //    var user = _users.FirstOrDefault(x => x.Id == id);

        //    if (user != null)
        //        user.Password = null;

        //    return user;
        //}
    }
}