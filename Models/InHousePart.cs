namespace InventoryManager.Models
{
    internal class InHousePart : Part
    {
        internal int MachineID { get; set; } = -1;

        internal InHousePart(string name, double price, int inventory, int min, int max, int machineID)
        {
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            MachineID = machineID;
        }
    }
}
