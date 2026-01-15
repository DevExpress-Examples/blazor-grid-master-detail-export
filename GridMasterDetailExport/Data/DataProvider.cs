namespace GridMasterDetailExport.Data; 
public class DataProvider {
    private static List<User> users = [
        new User(){ UserID = 1, UserName = "Alex" },
        new User(){ UserID = 2, UserName = "Kevin" },
        new User(){ UserID = 3, UserName = "Rose" },
    ];

    private static List<Order> orders = [
        new Order(){ OrderID = 1, UserID = 1, OrderDate = DateTime.Today.AddDays(1), ProductName = "Book" },
        new Order(){ OrderID = 2, UserID = 1, OrderDate = DateTime.Today.AddDays(2), ProductName = "Sugar" },
        new Order(){ OrderID = 3, UserID = 1, OrderDate = DateTime.Today.AddDays(3), ProductName = "Laptop" },
        new Order(){ OrderID = 4, UserID = 2, OrderDate = DateTime.Today.AddDays(1), ProductName = "Brick" },
        new Order(){ OrderID = 5, UserID = 2, OrderDate = DateTime.Today.AddDays(2), ProductName = "Stone" },
        new Order(){ OrderID = 6, UserID = 2, OrderDate = DateTime.Today.AddDays(3), ProductName = "Potato" },
        new Order(){ OrderID = 7, UserID = 2, OrderDate = DateTime.Today.AddDays(4), ProductName = "Cucumber" },
        new Order(){ OrderID = 8, UserID = 2, OrderDate = DateTime.Today.AddDays(5), ProductName = "Keyboard" },
        new Order(){ OrderID = 9, UserID = 3, OrderDate = DateTime.Today.AddDays(6), ProductName = "Pencil" },
        new Order(){ OrderID = 10, UserID = 3, OrderDate = DateTime.Today.AddDays(7), ProductName = "Mug" },
        new Order(){ OrderID = 11, UserID = 3, OrderDate = DateTime.Today.AddDays(8), ProductName = "Magnet" },
        new Order(){ OrderID = 12, UserID = 3, OrderDate = DateTime.Today.AddDays(9), ProductName = "Vitamins" },
    ];

    public static List<User> GetUsers() => users;

    public static List<Order> GetOrders() => orders;
}
