using Microsoft.AspNetCore.Mvc;
using Tests.Common;
using Tests.WebApi.Entities;

using static Tests.WebApi.dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tests.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<Items> itemsRepository;

        public ItemController(IRepository<Items> nitemsRepository)
        {
           itemsRepository = nitemsRepository;
        }


        // GET: api/<ItemController>
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                .Select(item => item.AsDto());
            return items;
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
               
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        // POST api/<ItemController>
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createdItemDto)
        {
            var item = new Items
            {
                Name = createdItemDto.Name,
                Description = createdItemDto.Description,
                Price = createdItemDto.Price,
                CreatedDate = DateTimeOffset.Now
            };
            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            //var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            //if (existingItem == null)
            //{
            //    return NotFound();
            //}
            //var updatedItem = existingItem with
            //{
            //    Name = updateItemDto.Name,
            //    Description = updateItemDto.Description,
            //    Price = updateItemDto.Price
            //};

            //var index = items.FindIndex(existingItem => existingItem.Id == id);
            //items[index] = updatedItem;

            return NoContent();
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAysnc(Guid id)
        {
            //var index = items.FindIndex(existingItem => existingItem.Id == id);
            //if (index < 0)
            //{
            //    return NotFound();
            //}
            //items.RemoveAt(index);
            var item = await itemsRepository.GetAsync(id);
            if (item==null)
            {
                NotFound();
            }
            await itemsRepository.RemoveAsync(item.Id);
            return NoContent();
        }
    }
}
