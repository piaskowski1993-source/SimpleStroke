using Microsoft.EntityFrameworkCore;
using SS.Api.Data;


namespace SS.Api;

public class UserStore
{
    private readonly SSDbContext _db;
    public UserStore(SSDbContext db)
    {
        _db = db;
    }
    
    public async Task<User> Create(string authSubject)
    {
        var user = new User();
        user.AssignOwner(authSubject);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Get(Guid id, string authSubject)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.AuthSubject == authSubject);
    }

}