using System.ComponentModel.DataAnnotations.Schema;

namespace TNOGS.Models
{
    [Table("products")]
    public class Products
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type_id")]
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public Types Types { get; set; }
        [Column("price")]
        public int Price { get; set; }
        [Column("availability")]
        public bool Availability { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }

    }
}
