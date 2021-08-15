using System.Collections.Generic;
using System.ComponentModel;
using InventoryManager.Models;

namespace InventoryManager
{
    internal static class Inventory
    {
        internal static BindingList<Product> Products { get; } = new BindingList<Product>();
        internal static BindingList<Part> Parts { get; } = new BindingList<Part>();
        internal static Queue<int> DeletedProductIDs { get; private set; } = new Queue<int>();
        internal static Queue<int> DeletedPartIDs { get; private set; } = new Queue<int>();

        /// <summary>
        /// Provide some initial data to be displayed.
        /// </summary>
        internal static void InitializeData()
        {
            AddProduct(new Product("Product 1", 9.99, 5, 1, 10, null));
            AddProduct(new Product("Product 2", 19.99, 15, 1, 10, null));
            AddProduct(new Product("Product 3", 7.99, 7, 1, 10, null));
            AddProduct(new Product("Product 4", 23.99, 23, 1, 10, null));

            AddPart(new InHousePart("Part 1", 2.99, 3, 2, 6, 12345));
            AddPart(new InHousePart("Part 2", 6.99, 7, 2, 6, 94685));
            AddPart(new OutsourcedPart("Part 3", 4.75, 5, 2, 6, "Company X"));
            AddPart(new OutsourcedPart("Part 4", 14.99, 12, 2, 6, "Company Y"));
            AddPart(new InHousePart("Part 5", 27.99, 32, 2, 6, 34675));
            AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
            AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
            AddPart(new OutsourcedPart("Part 6", 0.99, 10, 2, 6, "Company Z"));
            AddPart(new OutsourcedPart("Part 6", 0.99, 10, 2, 6, "Company ABC"));
            AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
            AddPart(new OutsourcedPart("Part 6", 0.99, 10, 2, 6, "Company DEF"));
            AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
        }

        /// <summary>
        /// Add a <paramref name="product"/> to the inventory's list of products
        /// </summary>
        /// <param name="product"></param>
        internal static void AddProduct(Product product)
        {
            Products.Add(product);
        }

        /// <summary>
        /// Remove a <paramref name="product"/> from the inventory's list of products
        /// </summary>
        /// <param name="product"></param>
        internal static bool RemoveProduct(int productID)
        {
            DeletedProductIDs.Enqueue(productID);
            return false;
        }

        /// <summary>
        /// Look up a product by it's <paramref name="productID"/>
        /// </summary>
        /// <param name="productID"></param>
        /// <returns>A <seealso cref="Product">Product</seealso></returns>
        internal static Product LookupProduct(int productID)
        {
            return null;
        }

        /// <summary>
        /// Updates an existing product with a matching <paramref name="productID"/> using information from a new <paramref name="product"/>
        /// </summary>
        /// <param name="product"></param>
        internal static void UpdateProduct(int productID, Product product)
        {

        }

        /// <summary>
        /// Add a <paramref name="part"/> to the inventory's list of parts
        /// </summary>
        /// <param name="part"></param>
        internal static void AddPart(Part part)
        {
            Parts.Add(part);
        }

        /// <summary>
        /// Removes a <paramref name="part"/> from the inventory's list of parts
        /// </summary>
        /// <param name="part"></param>
        internal static bool DeletePart(Part part)
        {
            DeletedPartIDs.Enqueue(part.PartID);
            return false;
        }

        /// <summary>
        /// Look up a product by it's <paramref name="partID"/>
        /// </summary>
        /// <param name="partID"></param>
        /// <returns>A <seealso cref="Part">Part</seealso></returns>
        internal static Part LookupPart(int partID)
        {
            return null;
        }

        /// <summary>
        /// Updates an existing part with a matching <paramref name="partID"/> using information from a new <paramref name="part"/>
        /// </summary>
        /// <param name="part"></param>
        internal static void UpdatePart(int partID, Part part)
        {

        }
    }
}
