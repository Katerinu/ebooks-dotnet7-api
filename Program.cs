using ebooks_dotnet7_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("ebooks"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ebook-API", Version = "v1" });
});



var app = builder.Build();
//swagger endpoints and settings.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ebok-API");
});



var ebooks = app.MapGroup("api/ebook");

// TODO: Add more routes
ebooks.MapPost("/", CreateEBookAsync);
ebooks.MapGet("/", GetAllEbooksAsync);
ebooks.MapPut("/{id}", UpdateBookAsync);
ebooks.MapPut("/{id}/change-availability", ChangeAvailabilitysync);
ebooks.MapPut("{id}/increment-stock", IncrementStockAsync);
ebooks.MapPost("/purchase", EbookPurchaseAsync);
ebooks.MapDelete("/{id}", DeleteEbookAsync);

app.Run();

// TODO: Add more methods
async Task<IResult> CreateEBookAsync(DataContext context)
{
    return Results.Ok();
}

async Task<IResult> GetAllEbooksAsync([FromQuery] string genre, [FromQuery] string author, [FromQuery] string format, DataContext context)
{
    return Results.Ok();
}

async Task<IResult> UpdateBookAsync(int id, DataContext context)
{
    return Results.Ok();
}

async Task<IResult> ChangeAvailabilitysync(int id, DataContext context)
{
    return Results.Ok();
}

async Task<IResult> IncrementStockAsync(int id, DataContext context)
{
    return Results.Ok();
}

async Task<IResult> EbookPurchaseAsync(DataContext context)
{
    return Results.Ok();
}

async Task<IResult> DeleteEbookAsync(int id, DataContext context)
{
    return Results.Ok();
}

