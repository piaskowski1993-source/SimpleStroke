public class UserStore
{
    private readonly Dictionary<Guid, User> _users = new();
    public User Create()
    {
        var user = new User();
        _users[user.Id] = user;
        return user;
    }

    public User? Get(Guid id)
    {
        return _users.TryGetValue(id, out var user) ? user : null;
    }
}    
