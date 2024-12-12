using Godot;
using System.Collections.Generic;

public static class PolygonHelper
{
    /// <summary>
    /// Generates the vertices of a regular polygon.
    /// </summary>
    /// <param name="vertexCount">The number of vertices of the polygon.</param>
    /// <param name="firstVertex">The position of the first vertex of the polygon.</param>
    /// <returns>A list of Vector2 points representing the polygon's vertices.</returns>
    public static Vector2[] CreateRegularPolygon(int vertexCount, Vector2 firstVertex)
    {
        if (vertexCount < 3)
        {
            vertexCount = 3;
        }

        List<Vector2> vertices = new List<Vector2>();

        // Add the first vertex
        vertices.Add(firstVertex);

        // Calculate the radius of the circle
        float radius = firstVertex.Length();

        // Calculate the angle between each vertex in radians
        float angleStep = Mathf.Tau / vertexCount;

        // Calculate the initial angle of the first vertex
        float initialAngle = firstVertex.Angle();

        // Generate the rest of the vertices
        for (int i = 1; i < vertexCount; i++)
        {
            float angle = initialAngle + i * angleStep;
            Vector2 vertex = new Vector2(
                radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle)
            );
            vertices.Add(vertex);
        }
        //GD.Print("v:" + vertices);
        return vertices.ToArray();
    }

    public static Vector2[] CreateStraightRegularPolygon(int vertexCount, Vector2 firstVertex)
    {
        if (vertexCount < 3)
        {
            vertexCount = 3;
        }

        List<Vector2> vertices = new List<Vector2>();

        // Calculate the radius of the circle
        float radius = firstVertex.Length();

        // Calculate the angle between each vertex in radians
        float angleStep = Mathf.Tau / vertexCount;

        // Adjust the initial angle so one side is parallel to the X-axis
        float initialAngle = Mathf.Pi / 2 - angleStep / 2;

        // Generate the vertices
        for (int i = 0; i < vertexCount; i++)
        {
            float angle = initialAngle + i * angleStep;
            Vector2 vertex = new Vector2(
                radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle)
            );
            vertices.Add(vertex);
        }

        return vertices.ToArray();;
    }
}
