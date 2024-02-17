using TodoWebService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddDomainServices();

builder.Services.AddTodoContext(builder.Configuration);

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