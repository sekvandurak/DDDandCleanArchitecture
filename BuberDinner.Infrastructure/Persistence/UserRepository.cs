using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();
    public User? GetUserByEmail(string email)
    {
        var user = _users.SingleOrDefault(u => u.Email == email);
        return user;
    }

    public void Add(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        _users.Add(user);
    }
    
}