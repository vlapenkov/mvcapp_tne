using System.ComponentModel.DataAnnotations;

namespace WebApi1
{
    public class Product
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

    }
}