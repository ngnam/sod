namespace StoreOrder.WebApplication.Data.Enums
{
    public enum TypeOrderStatus
    {
       Done = 1, // Đã thanh toán
       NewOrder, // Chưa thanh toán = Order mới tạo
       Confirmed, // Đã confirm
    }
}
