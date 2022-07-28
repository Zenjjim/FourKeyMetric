using devops_metrics.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

DotNetEnv.Env.Load();

if (Environment.GetEnvironmentVariable("APPLICATION_LEVEL") == "API")
{
    
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtConfig = builder.Configuration.GetSection("JwtConfig");
var configManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{jwtConfig["issuer"]}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());

var openidconfig = configManager.GetConfigurationAsync().Result;
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(_ =>
{
    _.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = jwtConfig["issuer"],
        ValidateIssuer = true,
        
        ValidAudience = jwtConfig["audience"],
        ValidateAudience = true,
        
        ValidateIssuerSigningKey = true,
        IssuerSigningKeys = openidconfig.SigningKeys,
        
        RequireExpirationTime = true,
        ValidateLifetime = true,
        RequireSignedTokens = true,
        
    };
});

var app = builder.Build();

app.UseCors(policy => policy.WithOrigins("http://localhost:3000", "https://weu-dev-metric-web-app.azurewebsites.net").AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
}

if (Environment.GetEnvironmentVariable("APPLICATION_LEVEL") == "CRON")
{
    new FetchDataService().FetchAllData();
}

