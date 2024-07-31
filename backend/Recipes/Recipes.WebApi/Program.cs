using Recipes.Application;
using Recipes.Infrastructure;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddAutoMapper( typeof( Program ).Assembly );
builder.Services.AddApplicationBindings();
builder.Services.AddInfrastructureBindings( builder.Configuration );
builder.Services.AddControllers();

builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowSpecificOrigin",
        builder => builder.WithOrigins( "http://localhost:5173" )
                          .AllowAnyHeader()
                          .AllowAnyMethod() );
} );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors( "AllowSpecificOrigin" );
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();