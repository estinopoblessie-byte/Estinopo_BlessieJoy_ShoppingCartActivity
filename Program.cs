using System;

class Product
{
    public int Id;
    public string Name;
    public string Category;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine(string.Format(
        "{0,2}. {1,-22} {2,-14} PHP {3,8:F2}   Stock: {4,4}",
        Id, Name, Category, Price, RemainingStock));
    }
}

class OrderHistory
{
    public string ReceiptNo;
    public string Date;
    public double FinalTotal;
}

class Program
{
    static int receiptCounter = 1;

    static void Main()
    {
        Product[] products =
        {
            new Product{Id=1, Name="Fuji Apple",      Category="Food",        Price=77,  RemainingStock=130},
            new Product{Id=2, Name="Gardenia Bread", Category="Food",        Price=85,  RemainingStock=45},
            new Product{Id=3, Name="Fresh Milk",     Category="Food",        Price=105, RemainingStock=30},
            new Product{Id=4, Name="Wireless Mouse", Category="Electronics", Price=500, RemainingStock=10},
            new Product{Id=5, Name="Keyboard",       Category="Electronics", Price=850, RemainingStock=4},
            new Product{Id=6, Name="T-Shirt",        Category="Clothing",    Price=250, RemainingStock=20},
            new Product{Id=7, Name="Jeans",          Category="Clothing",    Price=900, RemainingStock=8},
            new Product{Id=8, Name="Bioderm soap",           Category="Household",   Price=42,  RemainingStock=50},
            new Product{Id=9, Name="Zondrox",        Category="Household",   Price=150, RemainingStock=30},
            new Product{Id=10,Name="Keratin Plus Gold",        Category="Household",   Price=57,  RemainingStock=5}
        };

        int[] cart = new int[products.Length];

        OrderHistory[] history = new OrderHistory[100];
        int historyCount = 0;

        while (true)
        {
            Console.Clear();

            Console.WriteLine(new string('=', 65));
            Console.WriteLine("                 SHOPPING CART SYSTEM");
            Console.WriteLine(new string('=', 65));
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Cart Management");
            Console.WriteLine("5. Order History");
            Console.WriteLine("6. Exit");
            Console.WriteLine(new string('-', 65));
            Console.Write("Enter Choice: ");

            int choice;

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Invalid();
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddToCart(products, cart);
                    break;

                case 2:
                    SearchProduct(products);
                    break;

                case 3:
                    FilterCategory(products);
                    break;

                case 4:
                    CartMenu(products, cart, history, ref historyCount);
                    break;

                case 5:
                    ShowHistory(history, historyCount);
                    break;

                case 6:
                    Console.WriteLine("Thank you!");
                    return;

                default:
                    Invalid();
                    break;
            }
        }
    }

    static void AddToCart(Product[] products, int[] cart)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(new string('=', 75));
            Console.WriteLine("                           PRODUCT LIST");
            Console.WriteLine(new string('=', 75));
            Console.WriteLine(" ID  Product Name            Category        Price        Stock");
            Console.WriteLine(new string('-', 75));

            for (int i = 0; i < products.Length; i++)
                products[i].DisplayProduct();

            Console.WriteLine(new string('-', 75));
            Console.Write("Enter Product ID (0 to Back): ");

            int id;

            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Invalid();
                continue;
            }

            if (id == 0)
                return;

            if (id < 1 || id > products.Length)
            {
                Invalid();
                continue;
            }

            Product p = products[id - 1];

            Console.Write("Enter Quantity: ");

            int qty;

            if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
            {
                Invalid();
                continue;
            }

            if (qty > p.RemainingStock)
            {
                Console.WriteLine("Not enough stock.");
                Pause();
                continue;
            }

            cart[id - 1] += qty;
            p.RemainingStock -= qty;

            Console.WriteLine("Item added to cart.");

            if (!AskYesNo("Add another item? (Y/N): "))
                return;
        }
    }

    static void SearchProduct(Product[] products)
    {
        Console.Clear();

        Console.Write("Enter Product Name to Search: ");
        string search = Console.ReadLine().ToLower();

        Console.WriteLine("\nRESULT:");
        Console.WriteLine(new string('-', 65));

        bool found = false;

        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].Name.ToLower().Contains(search))
            {
                products[i].DisplayProduct();
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No product found.");

        Pause();
    }

    static void FilterCategory(Product[] products)
    {
        Console.Clear();

        Console.WriteLine("===== CATEGORY MENU =====");
        Console.WriteLine("1. Food");
        Console.WriteLine("2. Electronics");
        Console.WriteLine("3. Clothing");
        Console.WriteLine("4. Household");
        Console.Write("Enter Choice: ");

        int choice;

        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Invalid();
            return;
        }

        string cat = "";

        switch (choice)
        {
            case 1: cat = "Food"; break;
            case 2: cat = "Electronics"; break;
            case 3: cat = "Clothing"; break;
            case 4: cat = "Household"; break;
            default:
                Invalid();
                return;
        }

        Console.WriteLine("\nCATEGORY: " + cat);
        Console.WriteLine(new string('-', 65));

        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].Category == cat)
                products[i].DisplayProduct();
        }

        Pause();
    }

    static void CartMenu(Product[] products, int[] cart,
        OrderHistory[] history, ref int historyCount)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("===== CART MANAGEMENT =====");
            Console.WriteLine("1. View Cart");
            Console.WriteLine("2. Update Quantity");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Checkout");
            Console.WriteLine("6. Back");
            Console.Write("Enter Choice: ");

            int choice;

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Invalid();
                continue;
            }

            switch (choice)
            {
                case 1:
                    ViewCart(products, cart);
                    break;

                case 2:
                    UpdateCart(products, cart);
                    break;

                case 3:
                    RemoveItem(products, cart);
                    break;

                case 4:
                    ClearCart(products, cart);
                    break;

                case 5:
                    Checkout(products, cart, history, ref historyCount);
                    break;

                case 6:
                    return;

                default:
                    Invalid();
                    break;
            }
        }
    }

    static void ViewCart(Product[] products, int[] cart)
    {
        Console.Clear();

        Console.WriteLine("===== YOUR CART =====");

        bool empty = true;
        double total = 0;

        for (int i = 0; i < cart.Length; i++)
        {
            if (cart[i] > 0)
            {
                double sub = cart[i] * products[i].Price;

                Console.WriteLine(products[i].Name + " x" + cart[i] +
                " = PHP " + sub.ToString("F2"));

                total += sub;
                empty = false;
            }
        }

        if (empty)
            Console.WriteLine("Cart is empty.");
        else
            Console.WriteLine("Total: PHP " + total.ToString("F2"));

        Pause();
    }

    static void UpdateCart(Product[] products, int[] cart)
    {
        ViewCart(products, cart);

        Console.Write("Enter Product ID to Update: ");

        int id;

        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Invalid();
            return;
        }

        if (id < 1 || id > cart.Length || cart[id - 1] == 0)
        {
            Invalid();
            return;
        }

        Console.Write("Enter New Quantity: ");

        int newQty;

        if (!int.TryParse(Console.ReadLine(), out newQty) || newQty < 0)
        {
            Invalid();
            return;
        }

        int oldQty = cart[id - 1];

        if (newQty > oldQty)
        {
            int add = newQty - oldQty;

            if (add > products[id - 1].RemainingStock)
            {
                Console.WriteLine("Not enough stock.");
                Pause();
                return;
            }

            products[id - 1].RemainingStock -= add;
        }
        else
        {
            products[id - 1].RemainingStock += (oldQty - newQty);
        }

        cart[id - 1] = newQty;

        Console.WriteLine("Cart updated.");
        Pause();
    }

    static void RemoveItem(Product[] products, int[] cart)
    {
        ViewCart(products, cart);

        Console.Write("Enter Product ID to Remove: ");

        int id;

        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Invalid();
            return;
        }

        if (id < 1 || id > cart.Length || cart[id - 1] == 0)
        {
            Invalid();
            return;
        }

        products[id - 1].RemainingStock += cart[id - 1];
        cart[id - 1] = 0;

        Console.WriteLine("Item removed.");
        Pause();
    }

    static void ClearCart(Product[] products, int[] cart)
    {
        if (!AskYesNo("Clear all items? (Y/N): "))
            return;

        for (int i = 0; i < cart.Length; i++)
        {
            products[i].RemainingStock += cart[i];
            cart[i] = 0;
        }

        Console.WriteLine("Cart cleared.");
        Pause();
    }

    static void Checkout(Product[] products, int[] cart,
OrderHistory[] history, ref int historyCount)
    {
        Console.Clear();

        double grandTotal = 0;
        bool empty = true;

        Console.WriteLine(new string('=', 58));
        Console.WriteLine("                       RECEIPT");
        Console.WriteLine("Receipt No: " + receiptCounter.ToString("D4"));
        Console.WriteLine("Date: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"));
        Console.WriteLine(new string('=', 58));

        for (int i = 0; i < cart.Length; i++)
        {
            if (cart[i] > 0)
            {
                double subtotal = cart[i] * products[i].Price;

                Console.WriteLine(string.Format(
                "{0,-12} {1,-22} {2,3}   PHP {3,8:F2}",
                products[i].Category,
                products[i].Name,
                cart[i],
                subtotal));

                grandTotal += subtotal;
                empty = false;
            }
        }

        if (empty)
        {
            Console.WriteLine("Cart is empty.");
            Pause();
            return;
        }

        Console.WriteLine(new string('-', 58));

        Console.WriteLine("Grand Total:".PadRight(38) +
        "PHP " + grandTotal.ToString("F2"));

        double discount = 0;

        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;

            Console.WriteLine("Discount (10%):".PadRight(38) +
            "-PHP " + discount.ToString("F2"));
        }

        Console.WriteLine(new string('-', 58));

        double finalTotal = grandTotal - discount;

        Console.WriteLine("Final Total:".PadRight(38) +
        "PHP " + finalTotal.ToString("F2"));

        Console.WriteLine(new string('-', 58));

        double payment;

        while (true)
        {
            Console.Write("Enter payment amount: PHP ");

            if (double.TryParse(Console.ReadLine(), out payment))
            {
                if (payment >= finalTotal)
                    break;
            }

            Console.WriteLine("Insufficient / Invalid payment.");
        }

        double change = payment - finalTotal;

        Console.WriteLine("Payment:".PadRight(38) +
        "PHP " + payment.ToString("F2"));

        Console.WriteLine("Change:".PadRight(38) +
        "PHP " + change.ToString("F2"));

        Console.WriteLine(new string('=', 58));
        Console.WriteLine("Thank you for your purchase!");

        history[historyCount] = new OrderHistory();
        history[historyCount].ReceiptNo = receiptCounter.ToString("D4");
        history[historyCount].Date = DateTime.Now.ToString();
        history[historyCount].FinalTotal = finalTotal;

        historyCount++;
        receiptCounter++;

        for (int i = 0; i < cart.Length; i++)
            cart[i] = 0;

        LowStock(products);

        Pause();
    }

    static void LowStock(Product[] products)
    {
        Console.WriteLine("\nLOW STOCK ALERT");
        Console.WriteLine(new string('-', 40));

        bool found = false;

        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].RemainingStock <= 5)
            {
                Console.WriteLine(products[i].Name +
                " has only " +
                products[i].RemainingStock +
                " stock(s) left.");

                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No low stock items.");
    }

    static void ShowHistory(OrderHistory[] history, int count)
    {
        Console.Clear();

        Console.WriteLine("===== ORDER HISTORY =====");

        if (count == 0)
        {
            Console.WriteLine("No orders yet.");
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Receipt #" + history[i].ReceiptNo +
                " - PHP " +
                history[i].FinalTotal.ToString("F2"));
            }
        }

        Pause();
    }

    static bool AskYesNo(string message)
    {
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine().Trim().ToUpper();

            if (input == "Y")
                return true;

            if (input == "N")
                return false;

            Console.WriteLine("Please enter Y or N only.");
        }
    }

    static void Invalid()
    {
        Console.WriteLine("Invalid input.");
        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}