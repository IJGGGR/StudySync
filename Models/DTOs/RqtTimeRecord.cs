using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RqtTimeRecord
    {
        public int UserId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Stopped { get; set; }
        public TimeSpan Goal { get; set; }
        public required string Category { get; set; }
        public string[] Tags { get; set; } = [];
        public bool IsProductive { get; set; }
    }
}
