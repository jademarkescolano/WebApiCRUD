using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiSample.Class;
using WebApiSample.Models;

namespace WebApiSample.Services
{
    public class UserServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;
        private readonly AppSettings _appSetting;



        public UserServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
            _appSetting = appSettings.Value;
        }

        public async Task<List<user>> Login(string user, string pwd)
        {
            List<user> xuser = new List<user>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("SELECT * FROM user WHERE username = @user AND password = @pass", con)
                {
                    CommandType = CommandType.Text,
                };
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@user", user);
                com.Parameters.AddWithValue("@pass", pwd);
                var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                if (await rdr.ReadAsync().ConfigureAwait(false))
                {
                    xuser.Add(new user
                    {
                        userid = Convert.ToInt32(rdr["userid"]),
                        username = rdr["username"].ToString(),
                        password = rdr["password"].ToString(),

                    });
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                    new Claim (ClaimTypes.Name ,user),
                    new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    }),

                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.Aes128CbcHmacSha256)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    xuser[0].token = tokenHandler.WriteToken(token);
                }
                else
                {
                    xuser.Add(new user
                    {
                        userid = 0,
                        username = null,
                        password = null,
                        token = null
                    });
                }
            }
            return xuser;
        }
    }
}

