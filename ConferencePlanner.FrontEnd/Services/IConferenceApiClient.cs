﻿using ConferencePlanner.ConferenceDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferencePlanner.FrontEnd.Services
{
    public interface IConferenceApiClient
    {
        Task<List<SearchResult>> SearchAsync(string query);
        Task<List<SessionResponse>> GetSessionsAsync();
        Task<SessionResponse> GetSessionAsync(int id);
        Task<List<SpeakerResponse>> GetSpeakersAsync();
        Task<SpeakerResponse> GetSpeakerAsync(int id);
        Task PutSessionAsync(Session session);
        Task<bool> AddAttendeeAsync(Attendee attendee);
        Task<AttendeeResponse> GetAttendeeAsync(string name);
        Task DeleteSessionAsync(int id);

        Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name);

        Task AddSessionToAttendeeAsync(string name, int sessionId);

        Task RemoveSessionFromAttendeeAsync(string name, int sessionId);
    }
}
