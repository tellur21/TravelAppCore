using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalBookings { get; set; }
        public int TotalPackages { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int ActiveBookings { get; set; }
        public List<RevenueChartData> RevenueChart { get; set; } = new();
        public List<BookingStatusData> BookingsByStatus { get; set; } = new();
    }
    public class RevenueChartData
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class BookingStatusData
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
