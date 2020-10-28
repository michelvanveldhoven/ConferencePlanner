﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferencePlanner.Backend.Data
{
    public class Attendee : ConferenceDTO.Attendee
    {
        public virtual ICollection<SessionAttendee> SessionsAttendees { get; set; }
    }
}
