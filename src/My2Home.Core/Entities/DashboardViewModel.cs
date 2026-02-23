using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Core.Entities
{
    public class DashboardViewModel
    {
        public int TotalTenants { get; set; }
        public int TotalBeds { get; set; }
        public int TotalUnOccupiedBeds { get; set; }
        public int TenantHostelId { get; set; }
        public int RoomHostelId { get; set; }
    }
}
