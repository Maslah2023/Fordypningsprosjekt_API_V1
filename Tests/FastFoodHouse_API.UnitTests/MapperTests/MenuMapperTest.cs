using AutoMapper;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodHouse_API.Mapper;

namespace FastFoodHouse_API.UniTests.MapperTest
{
    public class MenuMapperTest
    {
        private readonly IMapper _mapper;

        public MenuMapperTest()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MenuMapper()));
            _mapper = configuration.CreateMapper();
        }


        [Fact]
        public void MapToDto_WhenMenu_Given_ShouldReturn_OrderHeaderDto()
        {
            //Arrange
            MenuItem menuItem = new MenuItem()
            {
                Id = 1,
                Image = "test.jpg",
                Category = "tesr",
                Description = "Description",
                Name = "Name",
                Price = 1,
                SpecialTag = "test"
                

            };

            var menuItemDto = _mapper.Map<MenuItemDTO>(menuItem);
            Assert.Equal(menuItem.Id, menuItemDto.Id);
            Assert.Equal(menuItem.Image, menuItemDto.Image);
            Assert.Equal(menuItem.Category, menuItemDto.Category);
            Assert.Equal(menuItem.Description, menuItemDto.Description);
            Assert.Equal(menuItem.Name, menuItemDto.Name);
            Assert.Equal(menuItem.Price, menuItemDto.Price);
            Assert.Equal(menuItem.SpecialTag, menuItemDto.SpecialTag);
        }
    }
}
