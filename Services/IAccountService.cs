using BusinessObjects.Models;

namespace Services;

public interface IAccountService
{
    AccountMember? Authenticate(string email, string password);
}
