using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SystemHealthDto
    {
        public bool DatabaseConnected { get; set; }
        public bool IdentityServiceHealthy { get; set; }
        public bool PaymentServiceHealthy { get; set; }
        public DateTime LastChecked { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Issues { get; set; } = new();
    }
}
