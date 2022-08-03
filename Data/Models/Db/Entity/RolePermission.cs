using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Entity
{
    [Table("role_permissions")]
    public class RolePermissions
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("role_id")]
        public Role Role { get; set; }

        [ForeignKey("permission_id")]
        public Permission Permission { get; set; }
    }
}
