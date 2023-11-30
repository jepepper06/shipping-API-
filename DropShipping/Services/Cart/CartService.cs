using DropShipping.DAOs;
using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public class CartService : ICartService
{
    public CartService(
        OrderDAO orderDAO,
        ItemDAO itemDAO){
        _orderDAO = orderDAO;
        _itemDAO = itemDAO;
    }
    private readonly OrderDAO _orderDAO;
    private readonly ItemDAO _itemDAO;
    public async Task<ErrorOr<CartResponseDTO>> GetCartResponseDTO(long userId)
    {
        var resultOrder = await _orderDAO.GetFirstByUserId(userId);
        
        if(resultOrder.IsError)
            return resultOrder.FirstError;

        var order = resultOrder.Value;
        
        return new CartResponseDTO(order!);
    }
    public async Task<ErrorOr<bool>> removeItemFromOrder(long itemId)
    {
        var result = await _orderDAO.GetOrderByItemId(itemId);
        
        if(result.IsError){
            return false;
        }

        var order = result.Value;

        var itemToRemove = order.Items!.Single(i => i.Id == itemId);

        var productPrice = itemToRemove.Product!.Price;

        var quantity = itemToRemove.Quantity;

        order.Total -= quantity*productPrice;

        var updateResult = await _orderDAO.Upsert(order);
        if(updateResult.IsError) 
            return false;
        
        var removeResult =  await _itemDAO.Delete(itemId);
        if(removeResult.IsError)
            return false;
    
        return true;
    }
}