﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn không thể để trống tiêu đề tin tức")]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(150)]

        public string Alias { get; set; }
        [StringLength(250)]
        public string Description { get; set; }

        [AllowHtml]
        [StringLength(250)]
        public string Detail { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}