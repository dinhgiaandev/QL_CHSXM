using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_ProductCategory")]
    public class ProductCategory : CommonAbstract
    {
        public ProductCategory()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Alias { get; set; }
        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Icon { get; set; }

        public bool IsActive { get; set; }  // Ensure IsActive property exists

        public ICollection<Product> Products { get; set; }
        public virtual ICollection<BookService> BookServices { get; set; }
    }
}
