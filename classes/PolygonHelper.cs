using Godot;
using System.Collections.Generic;

public static class PolygonHelper
{
    public static Vector2[] CreateRegularPolygon(int vertexCount, float sideLength)
    {
        if (vertexCount < 3)
        {
            GD.PrintErr("A polygon must have at least 3 vertices.");
            return null;
        }

        List<Vector2> vertices = new List<Vector2>();

        // Calculate the radius of the circumscribed circle using the side length
        float radius = sideLength / (2 * Mathf.Sin(Mathf.Pi / vertexCount));

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

        return vertices.ToArray();
    }

	/// <param name="vertexCount">The number of vertices of the polygon.</param>
	/// <returns>A new list of Vector2 points representing the rotated polygon's vertices.</returns>
	public static Vector2[] RotatePolygon(Vector2[] _vertices)
	{
		List<Vector2> vertices = new List<Vector2>(_vertices);
		int vertexCount = vertices.Count;
		if (vertices == null || vertexCount < 3)
		{
			GD.PrintErr("Invalid polygon data or vertex count.");
			return null;
		}
		float rotationAngle = Mathf.Tau / vertexCount; // 360 degrees in radians divided by vertex count

		List<Vector2> rotatedVertices = new List<Vector2>();

		foreach (Vector2 vertex in vertices)
		{
			float angle = vertex.Angle() + rotationAngle;
			float length = vertex.Length();
			Vector2 rotatedVertex = new Vector2(
				length * Mathf.Cos(angle),
				length * Mathf.Sin(angle)
			);
			rotatedVertices.Add(rotatedVertex);
		}

		return rotatedVertices.ToArray();
	}

    /// <summary>
	/// Centers a polygon around the origin.
	/// </summary>
	/// <param name="vertices">The vertices of the polygon to center.</param>
	/// <returns>A new list of Vector2 points representing the centered polygon's vertices.</returns>
	public static Vector2[] CenterPolygon(Vector2[] _vertices)
	{
		List<Vector2> vertices = new List<Vector2>( _vertices);
		if (vertices == null || vertices.Count == 0)
		{
			GD.PrintErr("Invalid polygon data.");
			return null;
		}

		// Calculate the centroid of the polygon
		Vector2 centroid = Vector2.Zero;
		foreach (Vector2 vertex in vertices)
		{
			centroid += vertex;
		}
		centroid /= vertices.Count;

		// Translate vertices so the centroid is at the origin
		List<Vector2> centeredVertices = new List<Vector2>();
		foreach (Vector2 vertex in vertices)
		{
			centeredVertices.Add(vertex - centroid);
		}
		//Position = centroid;

		return centeredVertices.ToArray();
	}

	public static Rect2 GetBounds(Vector2[] points, Rect2 overwriteX = new Rect2())
	{
		float minX = float.MaxValue, minY = float.MaxValue;
		float maxX = float.MinValue, maxY = float.MinValue;

		foreach (Vector2 point in points)
		{
			if (point.X < minX) minX = point.X;
			if (point.Y < minY) minY = point.Y;
			if (point.X > maxX) maxX = point.X;
			if (point.Y > maxY) maxY = point.Y;
		}
		if(overwriteX == new Rect2())
		{
			return new Rect2(new Vector2(minX, minY), new Vector2(maxX - minX, (maxY - minY)));
		}
		else
		{
			return new Rect2(new Vector2(overwriteX.Position.X, minY), new Vector2(overwriteX.Size.X, (maxY - minY)));
		}
	}
}
