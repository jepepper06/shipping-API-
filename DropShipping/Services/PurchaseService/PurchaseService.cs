namespace DropShipping.Services;

using DropShipping.DAOs;
using DropShipping.Models;
using ErrorOr;

public class PurchaseService : IPurchaseService
{
    private readonly ProductDAO _productDAO;
    private readonly ItemDAO _itemDAO; 
    private readonly OrderDAO _orderDAO;
    private readonly UserDAO _userDAO;
    public PurchaseService
        (ProductDAO productDAO, 
        ItemDAO itemDAO, 
        OrderDAO orderDAO,
        UserDAO userDAO)
    {
        _itemDAO = itemDAO;
        _productDAO = productDAO;
        _orderDAO = orderDAO;
        _userDAO = userDAO;
    }
    public async Task<ErrorOr<bool>> AddItemToOrder
        (long userId, 
        long productId, 
        int quantity)
    {
        var productResult = await _productDAO.GetById(productId);

        if(productResult.IsError)
            return productResult.FirstError;
        

        Product product = productResult.Value;
        double productPrice = product.Price;

        var orderResult = await _orderDAO.GetFirstByUserId(userId);

        if(orderResult.IsError)
            return orderResult.FirstError;

        Item item = new Item{
            ProductId = productId,
            Quantity = (uint) quantity, 
        };
        
        var order = orderResult.Value;
        if(order is not null)
        {
            if(order.PaymentStatus!.Payed is false){
                order.Items!.Add(item);
                order.Total += quantity * productPrice;
                await _orderDAO.Upsert(order);
                return true;
            }

            var paymentStatus = new PaymentStatus{
                PaymentMethod = PaymentMethod.NONEYET,
                Payed = false
            };
            
            order = new Order(){
                UserId = userId,
                Total = quantity*productPrice,
                Items = new List<Item>(),
                PaymentStatus = paymentStatus
            };
            order.Items.Add(item);
            return await SaveUtil(order);
        }
         order = new Order(){
            UserId = userId,
            Items = new List<Item>(),
            Total = quantity * productPrice
        };
        order.Create();
        order.Items.Add(item);
        order.PaymentStatus = new PaymentStatus(){
            PaymentMethod = PaymentMethod.NONEYET,
            Payed = false
        };
        return await SaveUtil(order);
    }
    private async Task<ErrorOr<bool>> SaveUtil(Order order){
        var result = await _orderDAO.Save(order);

        if(result.IsError)
            return result.FirstError;

        return result;
    }
}