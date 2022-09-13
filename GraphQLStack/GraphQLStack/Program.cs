using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GraphQL.Server.Ui.Voyager;
using GraphQLStack;
using GraphQLStack.Domain;
using GraphQLStack.Graphql;
using Microsoft.EntityFrameworkCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddDbContextFactory<Context>(options => options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddAuthorization();

builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddFiltering()
                .AddAuthorization();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
            ValidAudience = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("TokenSettings").GetValue<string>("Key")))
        };

        //options.RequireHttpsMetadata = false;
        //options.SaveToken = true;
    });




var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();

//app.UseHttpsRedirection();
//app.UseWebSockets();
app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/graphql");
    // endpoints.MapControllers();
});

//app.MapControllers();

app.UseGraphQLVoyager(new VoyagerOptions { GraphQLEndPoint = "/graphql" }, "/graphql-voyager");


app.Run();
