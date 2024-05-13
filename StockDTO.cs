using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet7_api;

/// <summary>
/// Represents an eBook entity.
/// </summary>
public class StockDTO
{
    [Range(1, int.MaxValue)]
    public  int Stock { get; set; }
}
