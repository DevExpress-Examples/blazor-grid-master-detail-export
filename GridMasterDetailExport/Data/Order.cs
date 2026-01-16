namespace GridMasterDetailExport.Data;
public class Order(
    int orderID,
    int userID,
    DateTime orderDate,
    string? productName) {
    public int OrderID { get; set; } = orderID;
    public DateTime OrderDate { get; set; } = orderDate;
    public int UserID { get; set; } = userID;
    public string? ProductName { get; set; } = productName;
}
