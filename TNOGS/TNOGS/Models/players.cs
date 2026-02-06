using System.ComponentModel.DataAnnotations.Schema;

namespace TNOGS.Models
{
    [Table("players")]
    public class Players
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nick_name")]
        public string NickName { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("balance")]
        public int Balance { get; set; }
    }
}
