namespace FinanceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("UserInformation")]
    public partial class UserInformation
    {
        [Key]
        public string UserId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        [StringLength(100)]
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile;

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
