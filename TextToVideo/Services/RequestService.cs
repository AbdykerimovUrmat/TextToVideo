using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Extensions;
using DAL.EF;
using DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Uploader.Services
{
    public class RequestService
    {
        private AppDbContext Context { get; set; }

        private UserService UserService { get; set; }

        private UserManager<User> UserManager { get; set; }

        public RequestService(AppDbContext context, UserService userService, UserManager<User> userManager)
        {
            Context = context;
            UserService = userService;
            UserManager = userManager;
        }

        public async Task<int> Add<T>(T model)
        {
            var entity = model.Adapt<Request>();
            Context.Requests.Add(entity);
            var userId = UserService.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            if (!CanSubmit(user.LastRequestUtc))
                throw new InnerException($"User {user.UserName} cannot send messages now, try tommorow!", "10007");
            
            user.LastRequestUtc = DateTime.UtcNow;
            await Context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<List<T>> Get<T>()
        {
            return await Context.Requests
                .AsNoTracking()
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetUnused<T>(int topn)
        {
            return await Context.Requests
                .AsNoTracking()
                .Where(x => !x.IsUsed)
                .OrderBy(x => x.CreatedUtc)
                .Take(topn)
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task SetUnused(int topn)
        {
            await Context.Requests
                .Where(x => !x.IsUsed)
                .OrderBy(x => x.CreatedUtc)
                .Take(topn)
                .ForEachAsync(x => x.IsUsed = true);
            Context.SaveChanges();
        }

        public bool CanSubmit(DateTime? requestDateTimeUtc)
        {
            if (requestDateTimeUtc.IsNull())
                return true;

            var utcNow = DateTime.UtcNow;
            var yesterdayDeadline = utcNow.AddDays(-1);
            yesterdayDeadline = yesterdayDeadline.Date.Add(new TimeSpan(14, 0, 0));

            if (requestDateTimeUtc <= yesterdayDeadline)
                return true;

            return false;
        }
    }
}
