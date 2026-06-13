using BusinessObjects.Models;

namespace Repositories;

public interface IAccountRepository
{
    AccountMember? GetByEmail(string email);
    AccountMember? GetById(string memberId);
}
