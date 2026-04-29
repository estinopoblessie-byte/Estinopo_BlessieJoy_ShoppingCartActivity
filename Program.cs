using System;

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }
    public int RemainingStock { get; set; }

    public void DisplayProduct()
    {
        Console.WriteLine(string.Format("[{0,-10}] {1,2}. {2,-20} ₱{3,8:F2} | Stock: {4,4}",
            Category, Id, Name, Price, RemainingStock));
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
    static int receiptCounter = 1; // Receipt number

    static void Main()
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Fuji Apple", Category = "Food", Price = 77.00, RemainingStock = 130 },
            new Product { Id = 2, Name = "Gardenia Bread", Category = "Food", Price = 85.00, RemainingStock = 45 },
            new Product { Id = 3, Name = "Alaska Fresh Milk", Category = "Food", Price = 105.00, RemainingStock = 30 },
            new Product { Id = 4, Name = "Bacon Strips (1 pack)", Category = "Food", Price = 155.00, RemainingStock = 15 },
            new Product { Id = 5, Name = "Hershey's Kisses", Category = "Food", Price = 199.00, RemainingStock = 30 },
            new Product { Id = 6, Name = "Ribeye Steak", Category = "Food", Price = 999.00, RemainingStock = 20 },
            new Product { Id = 7, Name = "Potato Marble", Category = "Food", Price = 130.00, RemainingStock = 20 },
            new Product { Id = 8, Name = "Zonrox Cleaner", Category = "Household", Price = 150.00, RemainingStock = 30 },
            new Product { Id = 9, Name = "Safeguard Soap", Category = "Household", Price = 42.00, RemainingStock = 50 },
            new Product { Id = 10, Name = "Keratin Plus Gold", Category = "Household", Price = 57.00, RemainingStock = 4 }
        };

        int[] cartQuantities = new int[10];
        const int MAX_CART_CAPACITY = 50;
        int totalCartItems = 0;

        Console.WriteLine("Welcome to the Store!");
        Console.WriteLine();

        while (true)
        {
            // Store Menu
            Console.WriteLine("\n===== STORE MENU =====");
            Console.WriteLine(new string('=', 65));
            Console.WriteLine("Cat      ID  Product              Price     Stock");
            Console.WriteLine(new string('-', 65));

            foreach (var p in products)
            {
                p.DisplayProduct();
            }

            Console.WriteLine(new string('=', 65));
            Console.WriteLine("Cart Status: " + totalCartItems + "/50 items");

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

            // Check cart capacity
            if (totalCartItems + qty > MAX_CART_CAPACITY)
            {
                Console.WriteLine("Cart is full! Maximum capacity reached.");
                continue;
            }

            // Add to cart
            cartQuantities[productNum - 1] += qty;
            selectedProduct.DeductStock(qty);
            totalCartItems += qty;

            Console.WriteLine("Added to cart!");
            Console.WriteLine();

            // Continue shopping with validation
            while (true)
            {
                Console.Write("Add more items? (YES/NO): ");
                string choice = Console.ReadLine().Trim().ToUpper();
                if (choice == "YES")
                {
                    break;
                }
                else if (choice == "NO")
                {
                    goto Checkout;
                }
                else
                {
                    Console.WriteLine("Please enter YES or NO only.");
                }
            }
        }

    Checkout:

        // Receipt with Number & Date
        PrintReceipt(products, cartQuantities, totalCartItems);

        // Updated Stock
        Console.WriteLine("\n===== UPDATED STOCK =====");
        Console.WriteLine(new string('=', 65));
        Console.WriteLine("Cat      ID  Product             Price     Stock");
        Console.WriteLine(new string('-', 65));

        foreach (var p in products)
        {
            p.DisplayProduct();
        }

        Console.WriteLine(new string('=', 65));

        // Low Stock Alert
        CheckLowStock(products);

        Console.WriteLine("\nThank you for shopping!");
        Console.ReadKey();
    }

    static void PrintReceipt(Product[] products, int[] cartQuantities, int totalItems)
    {
        // Receipt Header
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("                    RECEIPT");
        Console.WriteLine("Receipt No: " + receiptCounter.ToString("D4"));
        Console.WriteLine("Date: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"));
        Console.WriteLine(new string('=', 70));
        Console.WriteLine("Category     Product              Qty  Subtotal");
        Console.WriteLine(new string('-', 70));

        double grandTotal = 0;
        bool hasItems = false;

        for (int i = 0; i < products.Length; i++)
        {
            if (cartQuantities[i] > 0)
            {
                double subtotal = products[i].GetItemTotal(cartQuantities[i]);
                grandTotal += subtotal;
                hasItems = true;

                Console.WriteLine(string.Format("{0,-10} {1,-25} {2,3} ₱{3,8:F2}",
                    products[i].Category, products[i].Name, cartQuantities[i], subtotal));
            }
        }

        if (!hasItems)
        {
            Console.WriteLine("No items purchased.");
        }
        else
        {
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Grand Total:                        ₱" + grandTotal.ToString("F2"));

            double discount = 0;
            if (grandTotal >= 5000)
            {
                discount = grandTotal * 0.10;
                Console.WriteLine("Discount (10%):                    -" + discount.ToString("F2"));
            }

            double finalTotal = grandTotal - discount;
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("Final Total:                        ₱" + finalTotal.ToString("F2"));

            // Payment Section
            Console.WriteLine(new string('-', 70));
            Console.Write("Enter payment amount: ₱");
            while (true)
            {
                string paymentInput = Console.ReadLine();
                if (double.TryParse(paymentInput, out double payment) && payment >= finalTotal)
                {
                    double change = payment - finalTotal;
                    Console.WriteLine("Payment:                            ₱" + payment.ToString("F2"));
                    Console.WriteLine("Change:                             ₱" + change.ToString("F2"));
                    Console.WriteLine(new string('=', 70));
                    Console.WriteLine("Thank you for your purchase!");
                    receiptCounter++; // Increment for next receipt
                    break;
                }
                else
                {
                    Console.Write("Insufficient payment. Enter amount >= ₱" + finalTotal.ToString("F2") + ": ₱");
                }
            }
        }
        Console.WriteLine(new string('=', 70));
    }

    static void CheckLowStock(Product[] products)
    {
        const int REORDER_LEVEL = 5;
        Console.WriteLine("\n===== LOW STOCK ALERT =====");
        Console.WriteLine(new string('=', 50));

        bool hasLowStock = false;
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].RemainingStock <= REORDER_LEVEL)
            {
                Console.WriteLine(products[i].Name + " has only " + products[i].RemainingStock + " stocks left.");
                hasLowStock = true;
            }
        }

        if (!hasLowStock)
        {
            Console.WriteLine("All products have sufficient stock.");
        }
        Console.WriteLine(new string('=', 50));
    }
}