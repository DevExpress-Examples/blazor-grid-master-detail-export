namespace GridMasterDetailExport.Data;
public class Order {
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserID { get; set; }
    public string? ProductName { get; set; }
}
