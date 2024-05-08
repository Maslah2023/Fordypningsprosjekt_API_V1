using AutoMapper;
using FastFoodHouse_API.Mapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodHouse_API.UniTests.MapperTest
{
    public class OrderMapperTest
    {
        private readonly IMapper _mapper;

        public OrderMapperTest()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new OrderMapper()));
            _mapper = configuration.CreateMapper();
        }
        

        [Fact]
        public void MapToDto_WhenOrder_Given_ShouldReturn_OrderHeaderDto()
        {
            //Arrange
            OrderHeader orderHeader = new OrderHeader()
            {
                PickupName = "name",
                PickupEmail = "email",
                PickUpPhoneNumber = "12345",
                OrderDate = DateTime.Now,
                ApplicationUserId = "test",
                OrderTotal = 100,
                StripePaymentIntentID = "12345",
                Status = "pending",
                TotalItems = 1,



                Customer = new Customer()
                {
                    Name = "name",
                    Email = "email",
                    Address = "address",
                    City = "address",
                    Id = "id",
                    PhoneNumber = "12345"
                },
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        OrderHeaderId = 1,
                        OrderDetailsId = 2,
                        ItemName = "name",
                        Quantity = 1,
                        Price = 1,
                        MenuItemId = 1,
                        
                        

                        MenuItem = new MenuItem()
                        {
                            Name= "name",
                            Id= 1,
                            Image = "test.jpg",
                            Category = "pizza",
                            Description = "description",
                            Price = 100,
                            SpecialTag = "best seller",
                            
                        },
                        

                        
                    }
                }



            };

            var orderHeaderDto = _mapper.Map<OrderHeaderDTO>(orderHeader);
            // Assert
            Assert.Equal(orderHeader.PickupName, orderHeaderDto.PickupName);
            Assert.Equal(orderHeader.PickupEmail, orderHeaderDto.PickupEmail);
            Assert.Equal(orderHeader.PickUpPhoneNumber, orderHeaderDto.PickupPhoneNumber);
            Assert.Equal(orderHeader.ApplicationUserId, orderHeaderDto.ApplicationUserId);
            Assert.Equal(orderHeader.OrderTotal, orderHeaderDto.OrderTotal);
            Assert.Equal(orderHeader.StripePaymentIntentID, orderHeaderDto.StripePaymentIntentID);
            Assert.Equal(orderHeader.Status, orderHeaderDto.Status);
            Assert.Equal(orderHeader.TotalItems, orderHeaderDto.TotalItems);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Name, orderHeaderDto.OrderDetails[0].MenuItem.Name);
            Assert.Equal(orderHeader.OrderDetails.Count, orderHeaderDto.OrderDetails.Count);
            Assert.Equal(orderHeader.OrderDetails[0].ItemName, orderHeaderDto.OrderDetails[0].ItemName);
            Assert.Equal(orderHeader.OrderDetails[0].Quantity, orderHeaderDto.OrderDetails[0].Quantity);
            Assert.Equal(orderHeader.OrderDetails[0].Price, orderHeaderDto.OrderDetails[0].Price);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Name, orderHeaderDto.OrderDetails[0].MenuItem.Name);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Id, orderHeaderDto.OrderDetails[0].MenuItemId);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Image, orderHeaderDto.OrderDetails[0].MenuItem.Image);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Category, orderHeaderDto.OrderDetails[0].MenuItem.Category);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Description, orderHeaderDto.OrderDetails[0].MenuItem.Description);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.Price, orderHeaderDto.OrderDetails[0].MenuItem.Price);
            Assert.Equal(orderHeader.OrderDetails[0].MenuItem.SpecialTag, orderHeaderDto.OrderDetails[0].MenuItem.SpecialTag);

        }
    }
}
