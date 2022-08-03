using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Entity
{
    [Table("role")]
    public class Role
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public List<UserRoles> RoleUsers { get; set; }

        public List<RolePermissions> RolePermissions { get; set; }
    }
}
