public class UserTests
{
    [Fact]
    public void NewUser_IsAssignedShape()
    {
        var user = new User();

        Assert.NotEqual(Shape.None, user.Shape);
    }
}