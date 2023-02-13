using Application.Services.Repositories;
using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserEmailAuthenticatorRepository: EfRepositoryBase<UserEmailAuthenticator, BaseDbContext>,
                                               IUserEmailAuthenticatorRepository
{
    public UserEmailAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<UserEmailAuthenticator>> DeleteAllNonVerifiedAsync(User user)
    {
        List<UserEmailAuthenticator> userEmailAuthenticators = Query()
            .Where(uea => uea.UserId == user.Id && uea.IsVerified == false)
            .ToList();

        await DeleteRangeAsync(userEmailAuthenticators);

        //foreach (UserEmailAuthenticator userEmailAuthenticator in userEmailAuthenticators)
        //{
        //    Context.Entry(userEmailAuthenticator).State = EntityState.Deleted;
        //}
        //await Context.SaveChangesAsync();

        return userEmailAuthenticators;
    }
}