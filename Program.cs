using System;
using System.Collections.Generic;

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int RemainingStock { get; set; }

    public void DisplayProduct()
    {
        Console.WriteLine(string.Format("  {0,2}. {1,-25} ₱{2,8:F2} | Stock: {3,4}",
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

class CartItem
{
    public Product Product {get; set;}
    public int Quantity {get; set;}

    public double Subtotal
    {
        get { return Product.GetItemTotal(Quantity); }
    }
}

class ShoppingCart
{
    private List<CartItem> items = new List<CartItem>();
    private Product[] products;

    public ShoppingCart(Product[] products)
    {
        this.products = products;
    }
    public Product[] Products { get { return products; } }

    public void AddItem(int productId, int quantity)
    {
        Product product = products[productId - 1];
        if (product.HasEnoughStock(quantity))
        {
            CartItem existingItem = items.Find(item => item.Product.Id == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            product.DeductStock(quantity);
            Console.WriteLine("Added to cart!");
        }
    }

    public void DisplayCart()
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.WriteLine("\n===== SHOPPING CART =====");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine("Product Qty Subtotal");
        Console.WriteLine(new string('-', 55));

        double total = 0;
        foreach (var item in items)
        {
            Console.WriteLine(string.Format("{0,-25} {1,3} PHP{2,8:F2}",
                item.Product.Name, item.Quantity, item.Subtotal));
            total += item.Subtotal;
        }

        Console.WriteLine(new string('-', 55));
        Console.WriteLine("Total Items: " + items.Count + "PHP" + total.ToString("F2"));
        Console.WriteLine(new string('=', 55));
    }

    public void RemoveItem()
    {
        DisplayCart();
        if (items.Count == 0)
        {
            Console.WriteLine("Nothing to remove.");
            return;
        }

        Console.Write("Enter product ID to remove: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            CartItem item = items.Find(i => i.Product.Id == productId);
            if (item != null)
            {
                // Return stock
                item.Product.RemainingStock += item.Quantity;
                items.Remove(item);
                Console.WriteLine("Item removed from cart.");
            }
            else
            {
                Console.WriteLine("Item not found in cart.");
            }
        }
    }

    public void UpdateQuantity()
    {
        DisplayCart();
        if (items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.Write("Enter product ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            CartItem item = items.Find(i => i.Product.Id == productId);
            if (item != null)
            {
                Console.Write("Enter new quantity: ");
                if (int.TryParse(Console.ReadLine(), out int newQty) && newQty >= 0)
                {
                    int stockDifference = item.Quantity - newQty;
                    item.Product.RemainingStock += stockDifference;
                    item.Quantity = newQty;

                    if (newQty == 0)
                    {
                        items.Remove(item);
                        Console.WriteLine("Item removed from cart.");
                    }
                    else
                    {
                        Console.WriteLine("Quantity updated.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Item not found in cart.");
            }
        }
    }

    public void ClearCart()
    {
        foreach (var item in items)
        {
            item.Product.RemainingStock += item.Quantity;
        }
        items.Clear();
        Console.WriteLine("Cart cleared.");
    }

    public void Checkout()
    {
        DisplayCart();
        if (items.Count == 0)
        {
            Console.WriteLine("Nothing to checkout.");
            return;
        }

        double grandTotal = 0;
        foreach (var item in items) grandTotal += item.Subtotal;

        double discount = 0;

        Console.WriteLine(new string('-', 55));
        Console.WriteLine("Grand Total: PHP" + grandTotal.ToString("F2"));

        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine("Discount (10%): -" + discount.ToString("F2"));
        }

        double finalTotal = grandTotal - discount;
        Console.WriteLine(new string('=',
         55));
        Console.WriteLine("Final Total: PHP" + finalTotal.ToString("F2"));
        Console.WriteLine(new string('=', 55));

        // Clear cart after checkout
        items.Clear();
        Console.WriteLine("Checkout complete. Cart cleared.");
    }

    public List<CartItem> Items { get { return items; } }
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

        ShoppingCart cart = new ShoppingCart(products);

        Console.WriteLine("Welcome to the Enhanced Store!");
        Console.WriteLine();

        while (true)
        {
            // Main Menu
            Console.WriteLine("\n===== MAIN MENU =====");
            Console.WriteLine("1. View Store Menu");
            Console.WriteLine("2. Search Products");
            Console.WriteLine("3. View Cart");
            Console.WriteLine("4. Add to Cart");
            Console.WriteLine("5. Remove Item");
            Console.WriteLine("6. Update Quantity");
            Console.WriteLine("7. Clear Cart");
            Console.WriteLine("8. Checkout");
            Console.WriteLine("9. Exit");
            Console.Write("Choose option (1-9): ");

            string option = Console.ReadLine();
            Console.WriteLine();

            switch (option)
            {
                case "1":
                    DisplayStoreMenu(products);
                    break;

                case "2":
                    SearchProducts(products);
                    break;

                case "3":
                    cart.DisplayCart();
                    break;

                case "4":
                    AddToCart(cart);
                    break;

                case "5":
                    cart.RemoveItem();
                    break;

                case "6":
                    cart.UpdateQuantity();
                    break;

                case "7":
                    cart.ClearCart();
                    break;

                case "8":
                    cart.Checkout();
                    break;

                case "9":
                    Console.WriteLine("Thank you for shopping!");
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static void DisplayStoreMenu(Product[] products)
    {
        Console.WriteLine("===== STORE MENU =====");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine("ID  Product Price Stock");
        Console.WriteLine(new string('-', 55));

        foreach (var product in products)
        {
            product.DisplayProduct();
        }

        Console.WriteLine(new string('=', 55));
    }

    static void SearchProducts(Product[] products)
    {
        Console.Write("Enter product name to search: ");
        string searchTerm = Console.ReadLine().ToLower();

        bool found = false;
        Console.WriteLine("\nSearch Results:");
        Console.WriteLine(new string('-', 40));

        foreach (var product in products)
        {
            if (product.Name.ToLower().Contains(searchTerm))
            {
                Console.WriteLine(string.Format("{0}. {1,-25} ₱{2,8:F2} | Stock: {3}",
                    product.Id, product.Name, product.Price, product.RemainingStock));
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No products found.");
        }
        Console.WriteLine(new string('-', 40));
    }

    static void AddToCart(ShoppingCart cart)
    {
        DisplayStoreMenu(cart.Products);
        Console.Write("\nEnter product number (1-10): ");
        if (int.TryParse(Console.ReadLine(), out int productNum) && productNum >= 1 && productNum <= 10)
        {
            Product product = cart.Products[productNum - 1];
            if (product.RemainingStock > 0)
            {
                Console.Write("Enter quantity (1-" + product.RemainingStock + "): ");
                if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0 && qty <= product.RemainingStock)
                {
                    cart.AddItem(productNum, qty);
                }
                else
                {
                    Console.WriteLine("Invalid quantity!");
                }
            }
            else
            {
                Console.WriteLine("Product is out of stock!");
            }
        }
        else
        {
            Console.WriteLine("Invalid product number!");
        }
    }
}