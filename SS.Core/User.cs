public class User
{
    public Guid Id { get; }
    public Shape Shape { get; }
    public int UploadCount { get; private set; }
    public string? AuthSubject { get; private set; }
    public User()
    {
        Id = Guid.NewGuid();
        Shape = ShapeAssigner.AssignRandomShape();
    }

    public void Upload()
    {
        UploadCount++;
    }

    public void AssignOwner(string authSubject)
    {
        if (string.IsNullOrWhiteSpace(authSubject))
        {
            throw new ArgumentException("AuthSubject is required.", nameof(authSubject));
        }
        AuthSubject = authSubject;
    }
}