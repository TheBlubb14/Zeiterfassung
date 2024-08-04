using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(static x =>
{
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
