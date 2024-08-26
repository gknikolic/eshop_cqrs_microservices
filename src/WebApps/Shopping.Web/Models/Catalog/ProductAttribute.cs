using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Models.Catalog;

public class ProductAttribute
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Value { get; set; }
}
