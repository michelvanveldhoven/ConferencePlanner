using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.Backend.Data;
using ConferencePlanner.Backend.Infrastructure;
using ConferencePlanner.ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public SearchController(ApplicationDbContext context)
        {
            this.context =  context;
        }

        [HttpPost]
        public async Task<List<SearchResult>> Search(SearchTerm term)
        {
            var query = term.Query;
            var sessionResults = await context.Sessions.Include(s => s.Track)
                                                .Include(s => s.SessionSpeakers)
                                                    .ThenInclude(ss => ss.Speaker)
                                                .Where(s =>
                                                    s.Title.Contains(query) ||
                                                    s.Track.Name.Contains(query)
                                                )
                                                .ToListAsync();

            var speakerResults = await context.Speakers.Include(s => s.SessionSpeakers)
                                                    .ThenInclude(ss => ss.Session)
                                                .Where(s =>
                                                    s.Name.Contains(query) ||
                                                    s.Bio.Contains(query) ||
                                                    s.WebUrl.Contains(query)
                                                )
                                                .ToListAsync();

            var results = sessionResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Session,
                Session = s.MapSessionResponse()
            })
            .Concat(speakerResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Speaker,
                Speaker = s.MapSpeakerResponse()
            }));

            return results.ToList();
        }
    }
}
