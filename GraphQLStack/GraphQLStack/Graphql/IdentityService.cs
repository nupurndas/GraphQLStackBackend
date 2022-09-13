using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GraphQLStack.Domain;
using Microsoft.IdentityModel.Tokens;


namespace GraphQLStack.Graphql
{
    public interface IIdentityService
    {
        //Task<string> Authenticate([Service] Context context, string email, string password);
        string Authenticate([Service] Context context, string email, string password);
    }
    public class IdentityService : IIdentityService
    {
        //private readonly IIdentityService _identityService;
        //public async Task<string> Authenticate(string email, string password)
        public string Authenticate([Service] Context context, string email, string password)
        {
            //Your custom logic here (e.g. database query)
            
            //const user = await context.findaync(user with username and password)

            email = "test";
            password = "test";

            //AppUser login_user = new AppUser();
            //login_user.Email = email;
            //login_user.Password = password;

            //AppUser user1 = AuthenticateUser(login_user);
            //if( user1 != null ){
                 return GenerateAccessToken(email, password,  "1");
            //}

            throw new AuthenticationException();
        }

        private string GenerateAccessToken(string email,string password ,string userId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mysupersecret"));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("issuer",
              "audience",
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private AppUser AuthenticateUser(AppUser login)
        {
            AppUser user = new AppUser();

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.Email == "test")
            {
                user = new AppUser { Email = "test", Password = "test" };
            }
            return user;
        }

    }
}
