using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.ConferenceDTO;
using ConferencePlanner.FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConferencePlanner.FrontEnd.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IConferenceApiClient apiClient;

        public string SearchTerm { get; set; }

        public List<SearchResult> SearchResults { get; set; }

        public SearchModel(IConferenceApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        

        public async Task OnGetAsync(string searchTerm)
        {
            SearchTerm = searchTerm;
            SearchResults = await apiClient.SearchAsync(searchTerm);
        }
    }
}
