using System;

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int RemainingStock { get; set; }

    public void DisplayProduct()
    {
        Console.WriteLine(string.Format("  {0,2}. {1,-25} PHP {2,8:F2} | Stock: {3,4}",
            Id, Name, Price, RemainingStock));
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}

class Program
{
    static void Main()
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Fuji Apple", Price = 77.00, RemainingStock = 130 },
            new Product { Id = 2, Name = "Gardenia Bread", Price = 85.00, RemainingStock = 45 },
            new Product { Id = 3, Name = "Alaska Fresh Milk", Price = 105.00, RemainingStock = 30 },
            new Product { Id = 4, Name = "Bacon Strips (1 pack)", Price = 155.00, RemainingStock = 15 },
            new Product { Id = 5, Name = "Hershey's Kisses", Price = 199.00, RemainingStock = 30 },
            new Product { Id = 6, Name = "Ribeye Steak", Price = 999.00, RemainingStock = 20 },
            new Product { Id = 7, Name = "Potato Marble", Price = 130.00, RemainingStock = 20 },
            new Product { Id = 8, Name = "Zonrox Cleaner", Price = 150.00, RemainingStock = 30 },
            new Product { Id = 9, Name = "Safeguard Soap", Price = 42.00, RemainingStock = 50 },
            new Product { Id = 10,Name = "Keratin Plus Gold", Price = 57.00, RemainingStock = 40 }
        };

        Console.WriteLine("Welcome to the Store!");
        Console.WriteLine();

        while (true)
        {
            // Menu Display
            Console.WriteLine("\n===== STORE MENU =====");
            Console.WriteLine(new string('=', 55));
            Console.WriteLine("ID  Product                    Price     Stock");
            Console.WriteLine(new string('-', 55));

            foreach (var p in products)
            {
                p.DisplayProduct();
            }

            Console.WriteLine(new string('=', 55));

            // Product selection
            Console.Write("\nEnter product number (1-10): ");
            string input = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(input, out int productNum) ||
                productNum < 1 || productNum > 10)
            {
                Console.WriteLine("Invalid input! Please try again.");
                continue;
            }

            Product selectedProduct = products[productNum - 1];

            if (selectedProduct.RemainingStock == 0)
            {
                Console.WriteLine("Product is out of stock!");
                continue;
            }

            // Quantity input
            Console.Write("Enter quantity (1-" + selectedProduct.RemainingStock + "): ");
            string qtyInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(qtyInput, out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity! Must be 1 or more.");
                continue;
            }

            if (qty > selectedProduct.RemainingStock)
            {
                Console.WriteLine("Only " + selectedProduct.RemainingStock + " available!");
                continue;
            }

            // Process sale
            selectedProduct.DeductStock(qty);
            Console.WriteLine("\n" + qty + "x " + selectedProduct.Name + " sold!");
            Console.WriteLine("New stock: " + selectedProduct.RemainingStock);
            Console.WriteLine();

            // Continue prompt
            Console.Write("Continue shopping? (YES/NO): ");
            string choice = Console.ReadLine() ?? string.Empty;
            if (choice.Trim().ToUpper() != "YES")
            {
                break;
            }
        }

        Console.WriteLine("\nThank you for shopping!");
        Console.ReadKey();
    }
}