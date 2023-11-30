namespace DropShipping.Errors;
using ErrorOr;


public static class Errors
{
    public static class OrderDAO
    {
        public static Error OrderNotFound => Error.NotFound(
            code: "Error.Order.NotFound",
            description: "The Order you are getting appear not exist"
        );
        public static Error UserNotExist => Error.Conflict(
            code: "Error.Order.UserNotExist",
            description:"The User of the Order doesn't exist"
        );
        // public static Error 
    }
    public static class ItemDAO
    {
        public static Error ItemNotFound => Error.NotFound(
            code: "Error.Item.NotFound",
            description: "The Order is not Found"
        );
    }
    public static class ProductDAO
    {
        public static Error ProductNotFound => Error.NotFound(
            code: "Error.Product.NotFound",
            description:"The Product is not Found"
        );
    }
    public static class UserDAO 
    {
        public static Error UserNotFound => Error.NotFound(
            code: "Error.User.NotFound",
            description: "The User is not Found"
        );
    }
    public static class CityDAO
    {
        public static Error CityNotFound => Error.NotFound(
            code: "Error.City.NotFound",
            description: "The City is not Found"
        );
    }
    public static class OfficeDAO 
    {
        public static Error OfficeNotFound => Error.NotFound(
            code:"Error.Office.NotFound",
            description: "The Office is not Found"
        );
    }
    public static class PaymentStatusDAO
    {
        public static Error PaymentNotFound => Error.NotFound(
            code: "Error.Payments.NotFound",
            description: "The Payment is not Found"
        );
    }
    public static class RoleDAO 
    {
        public static Error RoleNotFound => Error.NotFound(
            code: "Error.Role.NotFound",
            description: "The Role is not Found"
        );
    }
    public static class ShipmentAgencyDAO
    {
        public static Error ShipmentAgencyNotFound => Error.NotFound(
            code: "Error.ShipmentAgency.NotFound",
            description:"The Shipment-Agency is not Found"
        );
    }
    public static class UserDataDAO 
    {
        public static Error UserDataNotFound => Error.NotFound(
            code:"Error.UserDataDAO.NotFound",
            description: "The Data of the user is not Found"
        );
    }
    public static class UserAddressDAO{
        public static Error UserAddressNotFound => Error.NotFound(
            code: "Error.UserAddress.NotFound",
            description: "The Address of User is not Found"
        );
    }
    public static class ShipmentStatusDAO
    {
        public static Error ShipmentStateNotFound => Error.NotFound(
            code: "Error.ShipmentState.NotFound",
            description: "The  ShipmentState is not Found"
        );
    }
    public static class TransportDAO
    {
        public static Error TransportNotFound => Error.NotFound(
            code: "Error.Transport.NotFound",
            description: "The  Transport is not Found"
        );
    }
}