using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RqtCalendarEvent
    {
        public required int         UserId      { get; set; }
        public required string      Title       { get; set; }
        public required string      Location    { get; set; }
        public required string      Note        { get; set; }
        public required DateTime    When        { get; set; }
        // public required DateTime At { get; set; }
        // public required DateTime To { get; set; }
    }
}
