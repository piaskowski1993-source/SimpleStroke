public static class ShapeAssigner
{
    private static readonly Shape[] AssignableShapes =
{
    Shape.Circle,
    Shape.Square,
    Shape.Triangle
};

public static Shape AssignRandomShape()
{
    var index = Random.Shared.Next(AssignableShapes.Length);
    return AssignableShapes[index];
}
}