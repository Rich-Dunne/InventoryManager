using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using InventoryManager.Models;

namespace InventoryManager
{
    internal static class Inventory
    {
        internal static BindingList<Product> Products { get; set; } = new BindingList<Product>();
        internal static BindingList<Part> Parts { get; set; } = new BindingList<Part>();
        internal static Queue<int> DeletedProductIDs { get; private set; } = new Queue<int>();
        internal static Queue<int> DeletedPartIDs { get; private set; } = new Queue<int>();

        /// <summary>
        /// Provide some initial data to be displayed.
        /// </summary>
        internal static void InitializeData()
        {
            AddPart(new InHousePart("Wheel", 2.99, 3, 2, 6, 12345));
            AddPart(new InHousePart("Seat", 6.99, 7, 2, 6, 94685));
            AddPart(new OutsourcedPart("Chain", 4.75, 5, 2, 6, "Company X"));
            AddPart(new OutsourcedPart("Handlebars", 14.99, 12, 2, 6, "Company Y"));

            AddProduct(new Product("Red Bicycle", 9.99, 5, 1, 10, new BindingList<Part>() { Parts.First() }));
            AddProduct(new Product("Blue Bicycle", 19.99, 15, 1, 10, new BindingList<Part>()));
            AddProduct(new Product("Green Bicycle", 7.99, 7, 1, 10, new BindingList<Part>()));
            AddProduct(new Product("Yellow Bicycle", 23.99, 23, 1, 10, new BindingList<Part>()));
        }

        /// <summary>
        /// Adds a <paramref name="product"/> to the inventory's list of products
        /// </summary>
        /// <param name="product"></param>
        internal static void AddProduct(Product product)
        {
            if(Products.Count == 0 || (Products.Count > 0 && Products.Last().ProductID == product.ProductID - 1))
            {
                Products.Add(product);
                return;
            }

            if (product.ProductID == 0 && Products.First().ProductID != 0)
            {
                Products.Insert(0, product);
                return;
            }

            var previousProduct = Products.FirstOrDefault(x => x.ProductID == product.ProductID - 1);
            Products.Insert(Products.IndexOf(previousProduct), product);
        }

        /// <summary>
        /// Removes a <paramref name="product"/> from the inventory's list of products
        /// </summary>
        /// <param name="product"></param>
        /// /// <returns>False if the product could not be removed</returns>
        internal static bool RemoveProduct(int productID)
        {
            var productToRemove = Products.FirstOrDefault(x => x.ProductID == productID);
            if(productToRemove == null)
            {
                return false;
            }

            DeletedProductIDs.Enqueue(productID);
            Products.Remove(productToRemove);
            return true;
        }

        /// <summary>
        /// Looks up a product by it's <paramref name="productID"/>
        /// </summary>
        /// <param name="productID"></param>
        /// <returns>A <seealso cref="Product">Product</seealso> or null</returns>
        internal static Product LookupProduct(int productID) => Products.FirstOrDefault(x => x.ProductID == productID);

        /// <summary>
        /// Updates an existing product with a matching <paramref name="productID"/> using information from a new <paramref name="product"/>
        /// </summary>
        /// <param name="product"></param>
        internal static void UpdateProduct(int productID, Product product)
        {
            var productToUpdate = Products.FirstOrDefault(x => x.ProductID == productID);
            if(productToUpdate == null)
            {
                Debug.WriteLine($"productToUpdate is null.");
                return;
            }

            productToUpdate.ProductID = product.ProductID;
            productToUpdate.Name = product.Name;
            productToUpdate.Inventory = product.Inventory;
            productToUpdate.Price = product.Price;
            productToUpdate.Min = product.Min;
            productToUpdate.Max = product.Max;
            productToUpdate.AssociatedParts.Clear();
            foreach(Part part in product.AssociatedParts)
            {
                productToUpdate.AddAssociatedPart(part);
            }
            Debug.WriteLine($"\"{productToUpdate.Name}\" updated.");
        }

        /// <summary>
        /// Adds a <paramref name="part"/> to the inventory's list of parts
        /// </summary>
        /// <param name="part"></param>
        internal static void AddPart(Part part)
        {
            if(Parts.Count == 0 || (Parts.Count > 0 && Parts.Last().PartID == part.PartID - 1))
            {
                Debug.WriteLine($"Added in condition 1");
                Parts.Add(part);
                return;
            }
            if (part.PartID == 0 && Parts.First().PartID != 0)
            {
                Debug.WriteLine($"Added in condition 2");
                Parts.Insert(0, part);
                return;
            }

            var previousPart = Parts.FirstOrDefault(x => x.PartID == part.PartID - 1);
            if (previousPart != null)
            {
                Parts.Insert(Parts.IndexOf(previousPart)+1, part);
            }
        }

        /// <summary>
        /// Removes a <paramref name="part"/> from the inventory's list of parts
        /// </summary>
        /// <param name="part"></param>
        /// /// <returns>False if the part could not be removed</returns>
        internal static bool DeletePart(Part part)
        {
            var partToRemove = LookupPart(part.PartID);
            if(partToRemove == null)
            {
                return false;
            }

            DeletedPartIDs.Enqueue(part.PartID);
            Parts.Remove(partToRemove);
            Debug.WriteLine($"Successfully deleted part");
            return true;
        }

        /// <summary>
        /// Looks up a product by it's <paramref name="partID"/>
        /// </summary>
        /// <param name="partID"></param>
        /// <returns>A <seealso cref="Part">Part</seealso> or null</returns>
        internal static Part LookupPart(int partID) => Parts.FirstOrDefault(x => x.PartID == partID);

        /// <summary>
        /// Updates an existing part with a matching <paramref name="partID"/> using information from a new <paramref name="part"/>
        /// </summary>
        /// <param name="part"></param>
        internal static void UpdatePart(int partID, Part part)
        {
            var partToUpdate = Parts.FirstOrDefault(x => x.PartID == partID);
            if(partToUpdate == null)
            {
                Debug.WriteLine($"partToUpdate is null.");
                return;
            }
            
            if(part.GetType() != partToUpdate.GetType())
            {
                UpdateAsNewPartType();
                return;
            }

            partToUpdate.PartID = part.PartID;
            partToUpdate.Name = part.Name;
            partToUpdate.Inventory = part.Inventory;
            partToUpdate.Price = part.Price;
            partToUpdate.Min = part.Min;
            partToUpdate.Max = part.Max;
            Debug.WriteLine($"\"{partToUpdate.Name}\" updated.");

            void UpdateAsNewPartType()
            {
                DeletePart(partToUpdate);
                DeletedPartIDs.Dequeue();
                AddPart(part);
                Debug.WriteLine($"\"{partToUpdate.Name}\" updated as new part \"{part.Name}\".");
            }
        }
    }
}
