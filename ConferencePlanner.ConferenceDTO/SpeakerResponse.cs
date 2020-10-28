﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConferencePlanner.ConferenceDTO
{
    public class SpeakerResponse : Speaker
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
