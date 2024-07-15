using Recipes.Application;

var builder = WebApplication.CreateBuilder( args );

// Регистрация других зависимостей
builder.Services.AddControllers();
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
