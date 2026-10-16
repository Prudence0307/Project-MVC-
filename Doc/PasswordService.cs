using Microsoft.AspNetCore.Identity;
using RaceDayMVC.Models;

namespace RaceDayMVC.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _hasher = new();
    public string Hash(string password) => _hasher.HashPassword(new User(), password);
    public bool Verify(string password, string hash) =>
        _hasher.VerifyHashedPassword(new User(), hash, password) != PasswordVerificationResult.Failed;
}
