using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Text;
using GraphQLStack.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLStack.Graphql
{
    public class Mutation
    {
        public async Task<Employee> AddEmployee([Service]Context dbContext,  Employee emp)
        {
            // Omitted code for brevity
            Employee newEmployee = new Employee
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                SupervisorId = emp.SupervisorId
            };

            dbContext.Employees.Add(emp);
            await dbContext.SaveChangesAsync();
            return emp;
        }

        public Task<Token> Login([Service] IOptions<TokenSettings> tokenSettings, [Service] Context context, string email, string password)
        {
           
            string token = GenerateAccessToken(tokenSettings,"test", "test", "1");
            Token token1 = new Token();
            token1.MyToken = token;
            return Task.FromResult( token1);

            throw new AuthenticationException();
        }

        private string GenerateAccessToken([Service] IOptions<TokenSettings> tokenSettings, string email, string password, string userId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSettings.Value.Key));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : tokenSettings.Value.Issuer,
                audience : tokenSettings.Value.Audience,
                expires: System.DateTime.Now.AddDays(2),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

