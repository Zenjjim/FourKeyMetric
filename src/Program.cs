using devops_metrics.Services;

DotNetEnv.Env.Load();

if (Environment.GetEnvironmentVariable("APPLICATION_LEVEL") == "API")
{
    
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors(policy => policy.WithOrigins("http://localhost:3000").AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
}

if (Environment.GetEnvironmentVariable("APPLICATION_LEVEL") == "CRON")
{
    new FetchDataService().FetchAllData();
}

