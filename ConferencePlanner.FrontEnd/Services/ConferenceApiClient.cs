using ConferencePlanner.ConferenceDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConferencePlanner.FrontEnd.Services
{
    public class ConferenceApiClient : IConferenceApiClient
    {
        private readonly HttpClient httpClient;

        public ConferenceApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<SearchResult>> SearchAsync(string query)
        {
            var term = new SearchTerm
            {
                Query = query
            };

            var response = await httpClient.PostAsJsonAsync($"/api/search", term);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SearchResult>>();
        }

        public async Task<bool> AddAttendeeAsync(Attendee attendee)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/attendees", attendee);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task DeleteSessionAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var response = await httpClient.GetAsync($"/api/attendees/{name}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<AttendeeResponse>();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            var response = await httpClient.GetAsync($"/api/sessions/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SessionResponse>();
        }

        public async Task<List<SessionResponse>> GetSessionsAsync()
        {
            var response = await httpClient.GetAsync("/api/sessions");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SessionResponse>>();
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            var response = await httpClient.GetAsync($"/api/speakers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SpeakerResponse>();
        }

        public async Task<List<SpeakerResponse>> GetSpeakersAsync()
        {
            var response = await httpClient.GetAsync("/api/speakers");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SpeakerResponse>>();
        }

        public async Task PutSessionAsync(Session session)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/sessions/{session.Id}", session);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name)
        {
            var response = await httpClient.GetAsync($"/api/attendees/{name}/sessions");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<SessionResponse>>();
        }

        public async Task AddSessionToAttendeeAsync(string name, int sessionId)
        {
            // https://localhost:5001/api/Attendees/michel%40mymail.com/session/59
            var response = await httpClient.PostAsync($"/api/Attendees/{name}/session/{sessionId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveSessionFromAttendeeAsync(string name, int sessionId)
        {
            var response = await httpClient.DeleteAsync($"/api/attendees/{name}/session/{sessionId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> CheckHealthAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync("/health");

                return string.Equals(response, "Healthy", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
