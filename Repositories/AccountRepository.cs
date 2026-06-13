using BusinessObjects.Models;
using DataAccessObjects;

namespace Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDAO _dao;

    public AccountRepository(AccountDAO dao)
    {
        _dao = dao;
    }

    public AccountMember? GetByEmail(string email) => _dao.GetByEmail(email);
    public AccountMember? GetById(string memberId) => _dao.GetById(memberId);
}
