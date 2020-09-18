namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        public Address LocationAddress { get; set; }
    }
}
