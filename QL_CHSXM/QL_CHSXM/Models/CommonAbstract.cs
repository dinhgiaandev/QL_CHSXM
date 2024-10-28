using System;
using System.ComponentModel.DataAnnotations;

namespace QL_CHSXM.Models
{
    public abstract class CommonAbstract
    {
        [StringLength(250)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [StringLength(250)]
        public string Modifiedby { get; set; }
    }
}
