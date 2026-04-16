using System;

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int RemainingStock { get; set; }

    public void DisplayProduct()
    {
        Console.WriteLine(string.Format("  {0,2}. {1,-25} PHP{2,8:F2} | Stock: {3,4}",
            Id, Name, Price, RemainingStock));
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
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
            new Product { Id = 10, Name = "Keratin Plus Gold", Price = 57.00, RemainingStock = 40 }
        };

        int[] cartQuantities = new int[10];
        string choice;

        Console.WriteLine("Welcome to the Store!");
        Console.WriteLine();

        while (true)
        {
            // Store Menu
            Console.WriteLine("\n===== STORE MENU =====");
            Console.WriteLine(new string('=', 55));
            Console.WriteLine("ID  Product                    Price     Stock");
            Console.WriteLine(new string('-', 55));

            foreach (var product in products)
            {
                product.DisplayProduct();
            }

            Console.WriteLine(new string('=', 55));

            // Product selection
            Console.Write("\nEnter product number (1-10): ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int productNum) ||
                productNum < 1 || productNum > 10)
            {
                Console.WriteLine("Invalid product number!");
                continue;
            }

            Product selectedProduct = products[productNum - 1];

            if (selectedProduct.RemainingStock == 0)
            {
                Console.WriteLine("This product is out of stock!");
                continue;
            }

            // Quantity input
            Console.Write("Enter quantity (1-" + selectedProduct.RemainingStock + "): ");
            string qtyInput = Console.ReadLine();

            if (!int.TryParse(qtyInput, out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity!");
                continue;
            }

            if (!selectedProduct.HasEnoughStock(qty))
            {
                Console.WriteLine("Not enough stock available!");
                continue;
            }

            // Add to cart
            cartQuantities[productNum - 1] += qty;
            selectedProduct.DeductStock(qty);

            Console.WriteLine("Added to cart!");
            Console.WriteLine();

            // Continue shopping?
            Console.Write("Add more items? (YES/NO): ");
            choice = Console.ReadLine().Trim().ToUpper();
            if (choice != "YES")
            {
                break;
            }
        }

        // Receipt
        Console.WriteLine("\n===== RECEIPT =====");
        Console.WriteLine(new string('=', 55));

        double grandTotal = 0;
        bool hasItems = false;

        for (int i = 0; i < products.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                double subtotal = products[i].GetItemTotal(cartQuantities[i]);
                grandTotal += subtotal;
                hasItems = true;

                Console.WriteLine(string.Format("{0,-28} x{1,2} = PHP{2,8:F2}",
                    products[i].Name, cartQuantities[i], subtotal));
            }
        }

        if (!hasItems)
        {
            Console.WriteLine("No items in cart.");
        }
        else
        {
            Console.WriteLine(new string('-', 55));
            Console.WriteLine("Grand Total: PHP" + grandTotal.ToString("F2"));

            // Discount
            double discount = 0;
            if (grandTotal >= 5000)
            {
                discount = grandTotal * 0.10;
                Console.WriteLine("Discount (10%): -PHP" + discount.ToString("F2"));
            }

            double finalTotal = grandTotal - discount;
            Console.WriteLine(new string('=', 55));
            Console.WriteLine("Final Total: PHP" + finalTotal.ToString("F2"));
            Console.WriteLine(new string('=', 55));
        }

        // Updated Stock
        Console.WriteLine("\n===== UPDATED STOCK =====");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine("ID  Product Price Stock");
        Console.WriteLine(new string('-', 55));

        foreach (var product in products)
        {
            product.DisplayProduct();
        }

        Console.WriteLine(new string('=', 55));

        Console.WriteLine("\nThank you for shopping!");
        Console.ReadKey();
    }
}

// FINALLY DONEE NAAA!! 