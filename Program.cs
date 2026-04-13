using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} | Stock: {RemainingStock}");
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
        Product[] products = new Product[3];

        products[0] = new Product { Id = 1, Name = "Apple", Price = 20, RemainingStock = 10 };
        products[1] = new Product { Id = 2, Name = "Bread", Price = 50, RemainingStock = 5 };
        products[2] = new Product { Id = 3, Name = "Milk", Price = 80, RemainingStock = 8 };

        int[] cartQty = new int[3];
        double grandTotal = 0;
        string choice = "Y";

        while (choice.ToUpper() == "Y")
        {
            Console.WriteLine("\n=== STORE MENU ===");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            bool isCartFull = true;
            for (int i = 0; i < cartQty.Length; i++)
            {
                if (cartQty[i] == 0)
                {
                    isCartFull = false;
                    break;
                }
            }

            if (isCartFull)
            {
                Console.WriteLine("Cart is full.");
                break;
            }

            Console.Write("\nEnter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int productNumber) ||
                productNumber < 1 || productNumber > products.Length)
            {
                Console.WriteLine("Invalid product number.");
                continue;
            }

            Product selectedProduct = products[productNumber - 1];

            if (selectedProduct.RemainingStock == 0)
            {
                Console.WriteLine("This product is out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (!selectedProduct.HasEnoughStock(quantity))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            double itemTotal = selectedProduct.GetItemTotal(quantity);

            cartQty[productNumber - 1] += quantity;

            selectedProduct.DeductStock(quantity);

            grandTotal += itemTotal;

            Console.WriteLine("Added to cart!");

            Console.Write("\nAdd more items? (Y/N): ");
            choice = Console.ReadLine();
        }

        Console.WriteLine("\n=== RECEIPT ===");
        for (int i = 0; i < products.Length; i++)
        {
            if (cartQty[i] > 0)
            {
                double subtotal = products[i].Price * cartQty[i];
                Console.WriteLine($"{products[i].Name} x {cartQty[i]} = ₱{subtotal}");
            }
        }

        Console.WriteLine($"\nGrand Total: ₱{grandTotal}");

        double finalTotal = grandTotal;
        if (grandTotal >= 5000)
        {
            double discount = grandTotal * 0.10;
            finalTotal -= discount;

            Console.WriteLine($"Discount (10%): ₱{discount}");
        }

        Console.WriteLine($"Final Total: ₱{finalTotal}");

        Console.WriteLine("\n=== UPDATED STOCK AFTER CHECKOUT ===");
        for (int i = 0; i < products.Length; i++)
        {
            products[i].DisplayProduct();
        }
    }
}