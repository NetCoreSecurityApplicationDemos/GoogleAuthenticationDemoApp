using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleAuthenticationDemoApp.Model
{
    [Table("Product")]
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }   

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
