using System;
using System.Collections.Generic;

namespace AikaHalli.Data
{
    public partial class UserTask
    {
        public UserTask()
        {
            TimeEntries = new HashSet<TimeEntry>();
        }

        public int TaskId { get; set; }
        public string? UserId { get; set; }
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }

        public virtual ICollection<TimeEntry> TimeEntries { get; set; }
    }
}
