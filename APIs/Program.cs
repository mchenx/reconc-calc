using Cargill.Reconc.Business;
using Cargill.Reconc.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<TradingsRepo>();
builder.Services.AddScoped<InsuranceRepo>();
builder.Services.AddScoped<CounterpartiesRepo>();
builder.Services.AddScoped<ReportsRepo>();
builder.Services.AddScoped<TradingBusinessLogic>();
builder.Services.AddScoped<CounterpartyBusinessLogic>();
builder.Services.AddScoped<InsuranceBusinessLogic>();
builder.Services.AddScoped<ReportBusinessLogic>();
builder.Services.AddScoped<ReconcCalculator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");
// app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
