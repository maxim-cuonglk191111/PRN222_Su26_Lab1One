using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects;

public class AccountDAO
{
    private readonly MyStoreContext _context;

    public AccountDAO(MyStoreContext context)
    {
        _context = context;
    }

    public AccountMember? GetByEmail(string email)
        => _context.AccountMembers.AsNoTracking()
            .FirstOrDefault(a => a.EmailAddress == email);

    public AccountMember? GetById(string memberId)
        => _context.AccountMembers.AsNoTracking()
            .FirstOrDefault(a => a.MemberID == memberId);
}
