using System.ComponentModel.DataAnnotations;

namespace PlumbworldDemo.Models.Products
{
    /// <summary>
    /// Product model, due to the simplicity of the model (no relationships for example) I'm not bothering with a separate Data Transfer Object
    /// </summary>
    public class Product
    {
        public int Id { get; set; } //EF defaults ID to be the PK and makes it auto incrementing, which is fine by me
        public string Name { get; set; } = null!;
        public string? Description { get; set; }    //description shouldn't be necessary, so letting it be nullable
        public decimal Price { get; set; }  //decimal for prices because accuracy and convention
        public int Stock { get; set; }  // the brief is unclear, normally I would confirm with the client if it is stock count, assuming it is
        [Display(Name="Is Active")]
        public bool IsActive { get; set; }
    }
}
