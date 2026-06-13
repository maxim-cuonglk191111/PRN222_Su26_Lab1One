using BusinessObjects.Models;
using Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repo;

    public AccountService(IAccountRepository repo)
    {
        _repo = repo;
    }

    public AccountMember? Authenticate(string email, string password)
    {
        var member = _repo.GetByEmail(email);
        if (member == null) return null;

        var hashed = HashPassword(password);
        return member.MemberPassword == hashed ? member : null;
    }

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
