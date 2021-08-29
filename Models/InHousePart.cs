namespace InventoryManager.Models
{
    internal class InHousePart : Part
    {
        internal int MachineID { get; set; }

        internal InHousePart(string name, double price, int inventory, int min, int max, int machineID)
        {
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            MachineID = machineID;
        }

        internal InHousePart(int partID, string name, double price, int inventory, int min, int max, int machineID)
        {
            PartID = partID;
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            MachineID = machineID;
        }
    }
}
