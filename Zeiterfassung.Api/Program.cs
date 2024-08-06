using Microsoft.EntityFrameworkCore;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Zeiterfassung.Api.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite("Data Source=Zeiterfassung.db"));

builder.Services.AddScoped<CallerContext>();
builder.Services.AddScoped<ApiKeyMiddleware>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(static x =>
{
    x.OperationFilter<SwaggerHeaderOperationFilter>();

    var documentationFile = Path.Combine(AppContext.BaseDirectory, "Zeiterfassung.Api.xml");
    if (File.Exists(documentationFile))
    {
        x.IncludeXmlCommentsWithRemarks(documentationFile);
        x.IncludeXmlCommentsFromInheritDocs(includeRemarks: true);
    }
});

var app = builder.Build();

app.MapOpenApi();
app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI(static x =>
{
    x.EnableTryItOutByDefault();
});

app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
