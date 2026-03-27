using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class DtoTimeRecord
    {
        public int UserId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Stopped { get; set; }
        public string? Category { get; set; }
        public bool IsProductive { get; set; }
    }
}
