using ConferencePlanner.FrontEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferencePlanner.FrontEnd.Services
{
    public interface IAdminService
    {
        Task<bool> AllowAdminUserCreationAsync();
    }

    public class AdminService : IAdminService
    {
        private readonly IServiceProvider serviceProvider;
        private bool adminExists;

        public AdminService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;       
        }

        public async Task<bool> AllowAdminUserCreationAsync()
        {
            if (adminExists)
            {
                return false;
            }
            else 
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                    if (await dbContext.Users.AnyAsync(user => user.IsAdmin))
                    {
                        adminExists = true;
                        return false;
                    }

                    return true;
                }
            }

        }
    }

    //public class AdminService : IAdminService
    //{
    //    private readonly IdentityDbContext identityContext;
    //    private bool _adminExists;

    //    public AdminService(IdentityDbContext context)
    //    {
    //        this.identityContext = context;
    //    }

    //    public async Task<bool> AllowAdminUserCreationAsync()
    //    {
    //        if (_adminExists)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            if (await identityContext.Users.AnyAsync(user => user.IsAdmin))
    //            {
    //                // There are already admin users so disable admin creation
    //                _adminExists = true;
    //                return false;
    //            }

    //            // There are no admin users so enable admin creation
    //            return true;
    //        }
    //    }
    //}
}   
