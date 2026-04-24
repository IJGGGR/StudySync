using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudySync.Models.DTOs;

namespace StudySync.Models
{
    public class MdlTimeRecord
    {
        public int      Id              { get; set; } = 0;
        public int      UserId          { get; set; } = 0;
        public DateTime Started         { get; set; } = DateTime.MinValue;
        public DateTime Stopped         { get; set; } = DateTime.MaxValue;
        public TimeSpan Length          { get; set; } = TimeSpan.Zero;
        public TimeSpan Goal            { get; set; } = TimeSpan.Zero;
        public string   Category        { get; set; } = "";
        public string[] Tags            { get; set; } = [];
        public bool     IsProductive    { get; set; } = false;
        public bool     IsDeleted       { get; set; } = false;

        // * CONSTRUCTORS ==================================================================================

        public MdlTimeRecord() { }

        public MdlTimeRecord(RqtTimeRecord rqt)
        {
            Id              = 0;
            UserId          = rqt.UserId;
            Started         = rqt.Started;
            Stopped         = rqt.Stopped;
            Length          = rqt.Stopped - rqt.Started;
            Goal            = rqt.Goal;
            Category        = rqt.Category;
            Tags            = rqt.Tags;
            IsProductive    = rqt.IsProductive;
            IsDeleted       = false;
        }

        // * METHODS =======================================================================================

        public void Update(RqtTimeRecord rqt)
        {
            // Id              = 0;
            UserId          = rqt.UserId;
            Started         = rqt.Started;
            Stopped         = rqt.Stopped;
            Length          = rqt.Stopped - rqt.Started;
            Goal            = rqt.Goal;
            Category        = rqt.Category;
            Tags            = rqt.Tags;
            IsProductive    = rqt.IsProductive;
            IsDeleted       = false;
        }
    }
}
