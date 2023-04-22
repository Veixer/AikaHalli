using System;
using System.Collections.Generic;

namespace AikaHalli.Data
{
    public partial class TimeEntry
    {
        public int EntryId { get; set; }
        public string? UserId { get; set; }
        public int? TaskId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? Notes { get; set; }

        public virtual UserTask? UserTask { get; set; }
    }
}
