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
    
    public async Task<User> Create()
    {
        var user = new User();
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Get(Guid id)
    {
        return await _db.Users.FindAsync(id);
    }

}