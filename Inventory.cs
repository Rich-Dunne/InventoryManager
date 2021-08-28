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
        internal static ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        internal static ObservableCollection<Part> Parts { get; set; } = new ObservableCollection<Part>();
        internal static Queue<int> DeletedProductIDs { get; private set; } = new Queue<int>();
        internal static Queue<int> DeletedPartIDs { get; private set; } = new Queue<int>();

        /// <summary>
        /// Provide some initial data to be displayed.
        /// </summary>
        internal static void InitializeData()
        {
            AddProduct(new Product("Product 1", 9.99, 5, 1, 10, new ObservableCollection<Part>() { new InHousePart("Part 1", 2.99, 3, 2, 6, 12345)  }));
            AddProduct(new Product("Product 2", 19.99, 15, 1, 10, new ObservableCollection<Part>()));
            AddProduct(new Product("Product 3", 7.99, 7, 1, 10, new ObservableCollection<Part>()));
            AddProduct(new Product("Product 4", 23.99, 23, 1, 10, new ObservableCollection<Part>()));

            AddPart(new InHousePart("Part 1", 2.99, 3, 2, 6, 12345));
            AddPart(new InHousePart("Part 2", 6.99, 7, 2, 6, 94685));
            AddPart(new OutsourcedPart("Part 3", 4.75, 5, 2, 6, "Company X"));
            AddPart(new OutsourcedPart("Part 4", 14.99, 12, 2, 6, "Company Y"));
            AddPart(new InHousePart("Part 5", 27.99, 32, 2, 6, 34675));
            //AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
            //AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
            //AddPart(new OutsourcedPart("Part 6", 0.99, 10, 2, 6, "Company Z"));
            //AddPart(new OutsourcedPart("Part 6", 0.99, 10, 2, 6, "Company ABC"));
            //AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
            //AddPart(new OutsourcedPart("Part 6", 0.99, 10, 2, 6, "Company DEF"));
            //AddPart(new InHousePart("Part 6", 0.99, 10, 2, 6, 90156));
        }

        /// <summary>
        /// Add a <paramref name="product"/> to the inventory's list of products
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
        /// Remove a <paramref name="product"/> from the inventory's list of products
        /// </summary>
        /// <param name="product"></param>
        internal static bool RemoveProduct(int productID)
        {
            DeletedProductIDs.Enqueue(productID);
            Products.Remove(Products.FirstOrDefault(x => x.ProductID == productID));
            return true;
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
            //productToUpdate = product;
            Debug.WriteLine($"\"{productToUpdate.Name}\" updated.");
        }

        /// <summary>
        /// Add a <paramref name="part"/> to the inventory's list of parts
        /// </summary>
        /// <param name="part"></param>
        internal static void AddPart(Part part)
        {
            if(Parts.Count == 0 || (Parts.Count > 0 && Parts.Last().PartID == part.PartID - 1))
            {
                Parts.Add(part);
                return;
            }
            if (part.PartID == 0 && Parts.First().PartID != 0)
            {
                Parts.Insert(0, part);
                return;
            }

            var previousPart = Parts.FirstOrDefault(x => x.PartID == part.PartID - 1);
            Parts.Insert(Parts.IndexOf(previousPart), part);
        }

        /// <summary>
        /// Removes a <paramref name="part"/> from the inventory's list of parts
        /// </summary>
        /// <param name="part"></param>
        internal static bool DeletePart(Part part)
        {
            DeletedPartIDs.Enqueue(part.PartID);
            Parts.Remove(Parts.FirstOrDefault(x => x.PartID == part.PartID));
            return true;
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
            var partToUpdate = Parts.FirstOrDefault(x => x.PartID == partID);
            if(partToUpdate == null)
            {
                Debug.WriteLine($"partToUpdate is null.");
                return;
            }
            Debug.WriteLine($"Part type is {part.GetType().Name}");

            if(part.GetType() != partToUpdate.GetType())
            {
                Debug.WriteLine($"Part types are different.");
                DeletePart(partToUpdate);
                DeletedPartIDs.Dequeue();
                AddPart(part);
                Debug.WriteLine($"\"{partToUpdate.Name}\" updated as new part.");
                return;
            }

            partToUpdate.PartID = part.PartID;
            partToUpdate.Name = part.Name;
            partToUpdate.Inventory = part.Inventory;
            partToUpdate.Price = part.Price;
            partToUpdate.Min = part.Min;
            partToUpdate.Max = part.Max;
            partToUpdate = part;
            Debug.WriteLine($"partToUpdate type is {partToUpdate.GetType().Name}");
            Debug.WriteLine($"\"{partToUpdate.Name}\" updated.");
        }
    }
}
