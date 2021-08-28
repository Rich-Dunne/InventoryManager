namespace InventoryManager.Models
{
    internal class OutsourcedPart : Part
    {
        internal string CompanyName { get; set; } = "";

        internal OutsourcedPart(string name, double price, int inventory, int min, int max, string companyName)
        {
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            CompanyName = companyName;
        }

        internal OutsourcedPart(int partID, string name, double price, int inventory, int min, int max, string companyName)
        {
            PartID = partID;
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            CompanyName = companyName;
        }
    }
}
