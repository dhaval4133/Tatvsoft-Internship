using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.Models.MissionsModels
{
    public class ApplyMissionRequestModel
    {
        public int UserId { get; set; } // The ID of the user applying for the mission
        public int MissionId { get; set; } // The ID of the mission to apply for
        public DateTime AppliedDate { get; set; } // The date when the application was made
        public int Sheet { get; set; } // Optional comments from the admin or user
        public bool Status { get; set; } // The status of the application (e.g., "Pending", "Accepted", "Rejected")
    }
}
