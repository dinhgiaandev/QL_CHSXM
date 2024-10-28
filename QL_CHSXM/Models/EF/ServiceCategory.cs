using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_ServiceCategory")]
    public class ServiceCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Alias { get; set; }

        public string Icon { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public int ServiceCategoryId { get; set; }

        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<BookService> BookServices { get; set; }
    }
}
