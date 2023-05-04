using System.ComponentModel.DataAnnotations;

namespace EDennis.MvcUtils
{
    public partial class AppUser : EntityBase
    {
        [Required]
        public string UserName { get; set; }

        public int? RoleId { get; set; }

        public AppRole AppRole { get; set; }

    }
}