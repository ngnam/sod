namespace StoreOrder.WebApplication.Data.Enums
{
    public enum TypeOrderDetail // = Status Product
    {
        Received = 1, // Đã nhận
        NewOrder, // Chưa nhận = Order mới
        Done, // Hoàn thành
        Cancel // Hủy bỏ
    }
}
