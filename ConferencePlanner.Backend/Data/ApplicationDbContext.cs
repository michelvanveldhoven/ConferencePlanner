using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferencePlanner.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionAttendee>()
                        .HasKey(ca => new { ca.SessionId, ca.AttendeeId });
            modelBuilder.Entity<SessionSpeaker>()
                        .HasKey(pk => new { pk.SessionId, pk.SpeakerId });
        }
        public DbSet<Attendee> Attendees { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Track> Tracks { get; set; }

    }
}
