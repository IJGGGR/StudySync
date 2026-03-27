using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models
{
    public class MdlTimeRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Stopped { get; set; }
        public TimeSpan Length { get; set; }
        public string? Category { get; set; }
        public bool IsProductive { get; set; }
    }
}
