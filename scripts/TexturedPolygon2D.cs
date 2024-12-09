using Godot;
using System;

public partial class TexturedPolygon2D : Polygon2D
{

	[Export]
	CollisionPolygon2D _CollisionPolygon2D {get;set;}

	[Export]
	WaveFunctionCollapseComponent _WaveFunctionCollapseComponent {get;set;}

	bool loaded = false;

	

	public override void _Process(double delta)
	{
		if(loaded == false)
		{
			load();
		}
		QueueRedraw();
	}

	public override void _Ready()
	{
		//load();
	}

	void load()
	{
		// Load and assign the texture
		var wfc = GetNode<WaveFunctionCollapseComponent>("WaveFunctionCollapseComponent");
		Texture = wfc.getOutputTexture();
		if(Texture != null)
		{
			// Set UV coordinates to map the polygon into the texture space
			UV = GenerateUVCoordinates(Polygon, Texture);
			_CollisionPolygon2D.Polygon = Polygon;
			loaded = true;
			QueueRedraw();
		}
	}

	public override void _Draw()
	{
		DrawColoredPolygon(this.Polygon,Colors.AliceBlue,UV,Texture);

		for(int i = 0;i<Polygon.Length;i++)
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

	private Vector2[] GenerateUVCoordinates(Vector2[] points, Texture texture)
	{
		// Calculate the bounding box of the polygon
		Rect2 bounds = GetBounds(points);

		// Normalize points to fit within texture dimensions
		Vector2[] uv = new Vector2[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			float u = (points[i].X - bounds.Position.X) / bounds.Size.X;
			float v = (points[i].Y - bounds.Position.Y) / bounds.Size.Y;

			uv[i] = new Vector2(u, v);
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

		return new Rect2(new Vector2(minX, minY), new Vector2(maxX - minX, maxY - minY));
	}
}
