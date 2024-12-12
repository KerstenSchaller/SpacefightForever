using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class TexturedPolygon2D : Node2D
{
	[Export]
	WaveFunctionCollapseComponent _WaveFunctionCollapseComponent {get;set;}

	[Export]
	bool UsePolyHelper{get;set;}
	[Export]
	int NumberOfVertices{get;set;}
	
	[Export]
	bool Snap{get;set;}

	CollisionPolygon2D collisionPoly;

	Vector2[] Polygon = new Vector2[3];
	Texture2D Texture;
	Vector2[] UV;

	bool loaded = false;
	public override void _Process(double delta)
	{
		if (loaded == false)
		{
			load();
		}

		if(UsePolyHelper)
		{
			var Polypoint = GetNode<Node2D>("Node2D");
			Polygon = PolygonHelper.CreateRegularPolygon(NumberOfVertices,Polypoint.Position);
		}
		collisionPoly.Polygon = Polygon;

		// Set UV coordinates to map the polygon into the texture space
		UV = GenerateUVCoordinates(Polygon);
		QueueRedraw();
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

	public override void _Ready()
	{
		collisionPoly = new CollisionPolygon2D();
	}

	void load()
	{
		// Load and assign the texture
		var wfc = _WaveFunctionCollapseComponent;
		//Texture = wfc.getOutputTexture();
		Texture = GD.Load<Texture2D>("res://output_image.png");
		if(Texture != null)
		{
			loaded = true;
		}
	}

	public override void _Draw()
	{
		if(Polygon.Length < 3)return;
		if (Engine.IsEditorHint())
		{
			// Code to execute when in editor.
			DrawPolygon(this.Polygon, new Color[] { Colors.DarkRed });
		}
		else
		{
			DrawColoredPolygon(this.Polygon, Colors.AliceBlue, UV, Texture);

		}

		for (int i = 0;i<Polygon.Length;i++)
		{
			if(i < Polygon.Length - 1)
			{
				DrawLine(Polygon[i],Polygon[i+1], GameColor.Color2,3);
			}
			else
			{
				DrawLine(Polygon[0],Polygon[i], GameColor.Color2,3);
			}

		}
	}

	private Vector2[] GenerateUVCoordinates(Vector2[] points)
	{
		// Calculate the bounding box of the polygon
		Rect2 bounds = GetBounds(points);

		// Normalize points to fit within texture dimensions
		Vector2[] uv = new Vector2[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			float u = (points[i].X - bounds.Position.X) / bounds.Size.X;
			float v = (points[i].Y - bounds.Position.Y) / (bounds.Size.Y);

			uv[i] = new Vector2(u/5, v/5);
		}
		return uv;
	}

	private Rect2 GetBounds(Vector2[] points)
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

		return new Rect2(new Vector2(minX, minY), new Vector2(maxX - minX, (maxY - minY)));
	}
}
