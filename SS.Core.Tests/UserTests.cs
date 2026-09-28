public class UserTests
{
    [Fact]
    public void NewUser_IsAssignedShape()
    {
        var user = new User();

        Assert.NotEqual(Shape.None, user.Shape);
    }
    [Fact]

        public void NewUser_StartsWithZeroUploads()
    {
        var user = new User();
        Assert.Equal(0, user.UploadCount);
    }
    [Fact]
    
    public void Upload_IncreaseesUpploadCount()
    {
        var user = new User();
        user.Upload();
        Assert.Equal(1, user.UploadCount);
    }
    [Fact]
    public void AssignOwner_SetsAuthSubject()
    {
        var user = new User();
        user.AssignOwner("apple");
        Assert.Equal("apple", user.AuthSubject);
    }
    [Fact]
    public void AssignOwner_RejectsEmptySubject()
    {
        var user = new User();
        Assert.Throws<ArgumentException>(() => user.AssignOwner(""));
    }
    }
