using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudySync.Models.DTOs;

namespace StudySync.Models
{
    public class MdlCalendarEvent
    {
        public int      Id          { get; set; }   = 0;
        public int      UserId      { get; set; }   = 0;
        public string   Title       { get; set; }   = "";
        public string   Location    { get; set; }   = "";
        public string   Note        { get; set; }   = "";
        public DateTime When        { get; set; }   = DateTime.MinValue;
        // public DateTime At          { get; set; }   = DateTime.UtcNow;
        // public DateTime To          { get; set; }   = DateTime.UtcNow;
        public bool     IsDeleted   { get; set; }   = false;

        // * CONSTRUCTORS ==================================================================================

        public MdlCalendarEvent() { }

        public MdlCalendarEvent(RqtCalendarEvent rqt)
        {
            Id          = 0;
            UserId      = rqt.UserId;
            Title       = rqt.Title;
            Location    = rqt.Location;
            Note        = rqt.Note;
            When        = rqt.When;
            // At          = rqt.At;
            // To          = rqt.To;
            IsDeleted   = false;
        }

        // * METHODS =======================================================================================

        public void Update(RqtCalendarEvent rqt)
        {
            // Id              = 0;
            UserId      = rqt.UserId;
            Title       = rqt.Title;
            Location    = rqt.Location;
            Note        = rqt.Note;
            When        = rqt.When;
            // At          = rqt.At;
            // To          = rqt.To;
            IsDeleted   = false;
        }
    }
}
