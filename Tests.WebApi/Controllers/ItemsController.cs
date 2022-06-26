using Microsoft.AspNetCore.Mvc;
using static Tests.WebApi.dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tests.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        // GET: api/<ItemsController>
        private readonly static List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(),"Potion","R",5,DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(),"Antidote","S",7,DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(),"Bronze sword","T",20,DateTimeOffset.Now)
        };


        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(item=>item.Id == id).SingleOrDefault();
            if (item==null)
            {
                return NotFound();
            }
            return item;
        }

        // POST api/<ItemsController>
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createdItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(),createdItemDto.Name,createdItemDto.Description,createdItemDto.Price, DateTimeOffset.Now);

            return CreatedAtAction(nameof(GetById), new {id=item.Id },item);
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem=items.Where(item=>item.Id == id).SingleOrDefault();
            if (existingItem==null)
            {
                return NotFound();
            }
            var updatedItem = existingItem with
            {
                Name=updateItemDto.Name,
                Description=updateItemDto.Description,
                Price=updateItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            if (index<0)
            {
                return NotFound();
            }
            items.RemoveAt(index);

            return NoContent();
        }
    }
}
