using Tests.WebApi.Entities;
using static Tests.WebApi.dto;

namespace Tests.WebApi
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Items item)
        {
            return new ItemDto(item.Id,item.Name,item.Description,item.Price,item.CreatedDate);
        }
    }
}
