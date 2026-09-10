public class User
{
    public Shape Shape { get; }
    public User()
    {
        Shape = ShapeAssigner.AssignRandomShape();
    }
}