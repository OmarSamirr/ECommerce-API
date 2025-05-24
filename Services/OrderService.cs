using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Baskets;
using Domain.Models.Orders;
using Domain.Models.Products;
using ServicesAbstraction;
using Shared.DataTransferObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(IBasketRepository _basketRepository,
                              IUnitOfWork _unitOfWork,
                              IMapper _mapper) : IOrderService
    {
        public async Task<OrderResponse> CreateAsync(OrderRequest orderRequest, string email)
        {
            var basket = await _basketRepository.GetAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);

            List<OrderItem> items = [];
            foreach (var item in basket.Items)
            {
                var originalProduct = await _unitOfWork.GetRepository<Product, int>()
                                            .GetByIdAsync(item.Id)
                                            ?? throw new ProductNotFoundException(item.Id);

                items.Add(CreateOrderItem(originalProduct, item));

            }
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                                  .GetByIdAsync(orderRequest.DeliveryMethodId)
                                                  ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var address = _mapper.Map<OrderAddress>(orderRequest.Address);

            var subTotal = items.Sum(i => (i.Quantity * i.Price));

            var order = new Order(items, address, subTotal, email, deliveryMethod);

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            orderRepo.Add(order);
            await _unitOfWork.SaveChanges();

            //Delete Basket After Creating Order
            await _basketRepository.DeleteAsync(orderRequest.BasketId);
            return _mapper.Map<OrderResponse>(order);
        }

        private OrderItem CreateOrderItem(Product originalProduct, BasketItem item)
        {
            return new OrderItem()
            {
                PictureUrl = originalProduct.PictureUrl,
                Price = originalProduct.Price,
                ProductName = originalProduct.Name,
                Quantity = item.Quantity,
                ProductId = originalProduct.Id,
            };
        }

        public Task<IEnumerable<OrderResponse>> GetAllAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod,int>()
                                            .GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);

        }
    }
}
