using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RqtTimeRecord
    {
        public required int         UserId          { get; set; }
        public required DateTime    Started         { get; set; }
        public required DateTime    Stopped         { get; set; }
        public required TimeSpan    Goal            { get; set; }
        public required string      Category        { get; set; }
        public required string[]    Tags            { get; set; }
        public required bool        IsProductive    { get; set; }
    }
}
