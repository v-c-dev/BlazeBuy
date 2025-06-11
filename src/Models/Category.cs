using System.ComponentModel.DataAnnotations;


namespace BlazeBuy.Models
{
    public sealed class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
    }
}
