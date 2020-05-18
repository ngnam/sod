namespace StoreOrder.WebApplication.Data.Enums
{
    public enum TypeOrderStatus
    {
        Received = 1, // Đã nhận
        NotReceived, // Chưa nhận
        Unprocessed, // Chưa chế biến
        Processed, // Đã chế biến
        Pendding, // Đang chờ / Tạo mới
        Done // Hoàng thành
    }
}
