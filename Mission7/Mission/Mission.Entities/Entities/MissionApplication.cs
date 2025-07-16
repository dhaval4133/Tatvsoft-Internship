using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.Entities
{
    [Table("MissionApplications")]
    public class MissionApplication : Base
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("mission_id")]
        public int MissionId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("applied_date")]
        public DateTime AppliedDate { get; set; }
        [Column("status")]
        public bool Status { get; set; } // e.g., "Pending", "Accepted", "Rejected"
        [Column("sheet")]
        public int Sheet { get; set; } // Optional comments from the admin or user
        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
        [ForeignKey(nameof(MissionId))]
        public virtual Missions Mission { get; set; } = null!;
    }
}
