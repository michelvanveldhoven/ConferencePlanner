using System;
using System.Collections.Generic;
using System.Text;

namespace ConferencePlanner.ConferenceDTO
{
    public class AttendeeResponse : Attendee
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
