using System.ComponentModel.DataAnnotations;

namespace EDennis.MvcUtils
{ 
    public class EntityBase : IHasIntegerId, IHasSysGuid, IHasSysUser
    {
        [Key]
        public int Id { get; set; }
        public string SysUser { get; set; }
        public Guid SysGuid { get; set; }
    }
}
