using DropShipping.Contracts;
using ErrorOr;

namespace DropShipping.Services;

public interface IPurchaseService{
    public Task<ErrorOr<bool>> AddItemToOrder(long userId, long productId, int quantity);
}