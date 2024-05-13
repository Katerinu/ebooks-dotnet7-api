using System.Net.Quic;
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
async Task<IResult> CreateEBookAsync([FromBody] CreateBookDTO inputBook ,DataContext context)
{
    var newbook = await context.EBooks.FirstOrDefaultAsync(x => x.Author == inputBook.Author && x.Title == inputBook.Title);


    if(newbook is null)
    {
        return Results.Conflict("Existe un libro con el mismo autor y titulo");
    }

    newbook.Stock = 0;
    newbook.IsAvailable = false;
    context.Add(newbook);
    await context.SaveChangesAsync();
    return Results.Ok(newbook);
}

async Task<IResult> GetAllEbooksAsync([FromQuery] string? genre, [FromQuery] string? author, [FromQuery] string? format, DataContext context)
{
    if(genre == null || author == null ||  format == null)
    {
        return Results.Ok(context.EBooks.ToArrayAsync());
    }

    return Results.Ok(await context.EBooks.Where(x => x.Genre == genre || x.Author == author || x.Format == format).ToListAsync());
}


async Task<IResult> UpdateBookAsync(int id,[FromBody] UpdateBookDTO updatedData ,DataContext context)
{
    var ebook = await context.EBooks.FindAsync(id);
    if(ebook is null)
    {
        return Results.NotFound("No se encontro la id");
    }
    if(updatedData.Title is not null){
        ebook.Title = updatedData.Title;
    }
    if(updatedData.Author is not null){
        ebook.Author = updatedData.Author;
    }
    if(updatedData.Genre is not null){
        ebook.Genre = updatedData.Genre;
    }
    if(updatedData.Format is not null){
        ebook.Format = updatedData.Format;
    }
    if(updatedData.Price > 0){
        ebook.Price = updatedData.Price;
    }
    await context.SaveChangesAsync();
    return Results.NoContent();
}

async Task<IResult> ChangeAvailabilitysync(int id, DataContext context)
{
    var ebook = await context.EBooks.FindAsync(id);
    if(ebook is null)
    {
        return Results.NotFound("No se encontro la id");
    }
    
    if(ebook.IsAvailable == true){
        ebook.IsAvailable = false;
    }
    else{
        ebook.IsAvailable = true;
    }
    await context.SaveChangesAsync();
    return Results.Ok(ebook);
}

async Task<IResult> IncrementStockAsync(int id, [FromBody] int ammount, DataContext context)
{
    var ebook = await context.EBooks.FindAsync(id);
    if(ebook is null)
    {
        return Results.NotFound("No se encontro la id");
    }

    ebook.Stock = ebook.Stock + ammount;
    await context.SaveChangesAsync();
    return Results.Ok(ebook);
}

async Task<IResult> EbookPurchaseAsync([FromBody] BuyBookDTO buyBook, DataContext context)
{
    var ebook = await context.EBooks.FindAsync(buyBook.Id);
    if(ebook is null)
    {
        return Results.NotFound("No se encontro la id");
    }

    if(ebook.Stock < buyBook.Quantity)
    {
        return Results.Conflict("El stock que se quiere comprar es mayor a la cantidad de stock actual.");
    }

    var CostoCalculado = ebook.Price*buyBook.Quantity;

    if(CostoCalculado != buyBook.Price)
    {
        return Results.Conflict("El precio pagado no corresponde al valor que se debe pagar.");
    }
    
    return Results.Ok(ebook);
}

async Task<IResult> DeleteEbookAsync(int id, DataContext context)
{
    var ebook = await context.EBooks.FindAsync(id);
    if(ebook is null)
    {
        return Results.NotFound("No se encontro la id");
    }

    context.EBooks.Remove(ebook);
    await context.SaveChangesAsync();
    return Results.NoContent();
}

