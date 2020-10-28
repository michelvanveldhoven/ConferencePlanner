using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferencePlanner.Backend.Data
{
    public class Track : ConferenceDTO.Track
    {
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
