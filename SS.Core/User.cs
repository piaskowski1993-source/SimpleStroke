public class User
{
    public Shape Shape { get; }
    public int UploadCount { get; private set; }
    public User()
    {
        Shape = ShapeAssigner.AssignRandomShape();
    }

    public void Upload()
    {
        UploadCount++;
    }
}