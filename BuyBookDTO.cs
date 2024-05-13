using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet7_api;

/// <summary>
/// Represents an eBook entity.
/// </summary>
public class BuyBookDTO
{
    /// <summary>
    /// Unique identifier for the eBook.
    /// </summary>
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public  int Price { get; set; }

        [Range(1, int.MaxValue)]
    public  int Quantity { get; set; }
}
