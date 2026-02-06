using System.ComponentModel.DataAnnotations.Schema;

namespace TNOGS.Models
{
    [Table("types")]
    public class Types
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("type_name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
