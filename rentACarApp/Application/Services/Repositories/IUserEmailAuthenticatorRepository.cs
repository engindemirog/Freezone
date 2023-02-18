using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Entities;

namespace Application.Services.Repositories;

public interface IUserEmailAuthenticatorRepository : IRepository<UserEmailAuthenticator>, IAsyncRepository<UserEmailAuthenticator>
{
    public Task<ICollection<UserEmailAuthenticator>> DeleteAllNonVerifiedAsync(User user);
}