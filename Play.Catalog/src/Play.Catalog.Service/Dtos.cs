using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
  public record ItemDto(Guid id, string name, string desc, decimal price, DateTimeOffset createdDate);

  public record CreateItemDto([Required] string name, string desc, [Range(0, 1000)] decimal price);

  public record UpdateItemDto([Required] string name, string desc, [Range(0, 1000)] decimal price);
  
}