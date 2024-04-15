using api_validacao_negocio.Services.VerificaPrazoDoPrecoFornecedor;
using api_validacao_negocio.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
CacheSettings cacheSettings = builder.Configuration.GetSection("CacheSettings").Get<CacheSettings>()!;
builder.Services.AddSingleton(cacheSettings);

builder.Services.AddScoped<ItemService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
