using Application.Services.Repositories;
using Freezone.Core.Persistence.Repositories;
using Freezone.Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserOtpAuthenticatorRepository : EfRepositoryBase<UserOtpAuthenticator, BaseDbContext>,
                                              IUserOtpAuthenticatorRepository
{
    public UserOtpAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }
}