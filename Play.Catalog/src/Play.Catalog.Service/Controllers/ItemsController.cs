using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase {
    
    private readonly IRepository<Item> itemsRepository;

    public ItemsController(IRepository<Item> itemsRepository) {
      this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> getAsync() {
      var items = (await itemsRepository.getAllAsync()).Select(item => item.AsDto());
      return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> getItemDtoById(Guid id) {
      var item = await itemsRepository.getAsync(id);
      if (item == null) return NotFound();
      return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> createItemDto(CreateItemDto newItemDto) {
      var item = new Item{
        name = newItemDto.name, 
        desc = newItemDto.desc,
        price = newItemDto.price,
        createdDate = DateTimeOffset.UtcNow
      };
      await itemsRepository.createAsync(item);
      return CreatedAtAction(nameof(getItemDtoById), new {id = item.id}, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> updateItemDto(Guid id, UpdateItemDto updateItemDto) {
      var existingItem = await itemsRepository.getAsync(id); 
      if (existingItem == null) return NotFound();
      existingItem.name = updateItemDto.name;
      existingItem.desc = updateItemDto.desc;
      existingItem.price = updateItemDto.price;

      await itemsRepository.updateAsync(existingItem);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteItemDto(Guid id) {
      var item = await itemsRepository.getAsync(id);
      if (item == null) return NotFound();
      await itemsRepository.deleteAsync(id);
      return NoContent();
    }
  }
  
}