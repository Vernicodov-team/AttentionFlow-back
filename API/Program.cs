var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "api/v1/docs"; 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});
app.Run();

