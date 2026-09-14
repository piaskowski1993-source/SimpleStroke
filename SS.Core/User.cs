public class User
{
    public Guid Id { get; }
    public Shape Shape { get; }
    public int UploadCount { get; private set; }
    public User()
    {
        Id = Guid.NewGuid();
        Shape = ShapeAssigner.AssignRandomShape();
    }

    public void Upload()
    {
        UploadCount++;
    }
}