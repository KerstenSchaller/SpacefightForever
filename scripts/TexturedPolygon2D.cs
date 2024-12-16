using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

[Tool]
public partial class TexturedPolygon2D : Node2D
{
	public WaveFunctionCollapseComponent _WaveFunctionCollapseComponent { get; set; }

	[Export]
	public NodePath WFCPath{get;set;}

	[Export]
	public Polygon2D polygon2D { get; set; }

	[Export]
	public bool UsePolyHelper { get; set; }
	[Export]
	public int NumberOfVertices { get; set; }

	[Export(PropertyHint.Range, "0,150,0.1")]
	public float SideLength { get; set; }

	
	CollisionPolygon2D CollisionPoly;

	Vector2[] Polygon = new Vector2[3];
	Texture2D Texture;
	Vector2[] UV;

	bool isCollisionPolySet = false;

	bool loaded = false;
	public override void _Process(double delta)
	{
		if (loaded == false)
		{
			load();
		}

		if (UsePolyHelper)
		{
			try
			{
				Polygon = PolygonHelper.CreateRegularPolygon(NumberOfVertices, SideLength);
			}
			catch
			{
				return;
			}
		}
		else
		{
			if (polygon2D != null)
			{
				Polygon = polygon2D.Polygon;
				for (int i = 0; i < Polygon.Length; i++)
				{
					Polygon[i] = Polygon[i] - Position;
				}
			}
		}
		if (CollisionPoly != null)
		{
			CollisionPoly.Polygon = Polygon;
		}



		// Set UV coordinates to map the polygon into the texture space
		//var rect = ScalePolygonToMaxWidth(Polygon,new Rect2(new Vector2(),new Vector2(200,200)));
		//UV = GenerateUVCoordinates(Polygon,PolygonHelper.GetBounds(Polygon));
		var rect = new Rect2(new Vector2(0,0),new Vector2(1000,1000));
		UV = GenerateUVCoordinates(Polygon,rect, 2f);
		QueueRedraw();
	}





	public override void _Ready()
	{
		CollisionPoly = GetNode<CollisionPolygon2D>("../CollisionPolygon2D");
		if(WFCPath != "")
		{
			_WaveFunctionCollapseComponent = GetNode<WaveFunctionCollapseComponent>(WFCPath);
		}
		else
		{
			GD.PrintErr("WFCPath not set");
		}
	}

	void load()
	{
		if(_WaveFunctionCollapseComponent == null)return;
		// Load and assign the texture
		if (!Engine.IsEditorHint())
		{
			var wfc = _WaveFunctionCollapseComponent;
			Texture = wfc.getOutputTexture();
		}
		else
		{
			Texture = GD.Load<Texture2D>("res://output_image.png");
		}
		if (Texture != null)
		{
			loaded = true;
		}
	}

	public override void _Draw()
	{
		if (Polygon.Length < 3) return;
		if (Engine.IsEditorHint())
		{
			// Code to execute when in editor.
			DrawPolygon(this.Polygon, new Color[] { Colors.DarkRed });
		}
		else
		{
			DrawColoredPolygon(this.Polygon, Colors.AliceBlue, UV, Texture);
		}

		for (int i = 0; i < Polygon.Length; i++)
		{
			if (i < Polygon.Length - 1)
			{
				DrawLine(Polygon[i], Polygon[i + 1], GameColor.Color2, 3);
			}
			else
			{
				DrawLine(Polygon[0], Polygon[i], GameColor.Color2, 3);
			}

		}
	}

	private Vector2[] GenerateUVCoordinates(Vector2[] points, Rect2 bounds, float scale)
	{
		//minimal values used for shifting
		float minU = 0;
		float minV = 0;

		// Normalize points to fit within texture dimensions
		Vector2[] uv = new Vector2[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			float u = (points[i].X - bounds.Position.X) / bounds.Size.X;
			float v = (points[i].Y - bounds.Position.Y) / bounds.Size.Y;
			if(u < minU) minU = u;
			if(v < minV) minV = v;

			uv[i] = new Vector2(u / scale, v / scale);
		}

		for (int i = 0; i < points.Length; i++)
		{
			uv[i] -= new Vector2(minU,minV);
		}
		return uv;
	}

	public Vector2[] ScalePolygonToMaxWidth(Vector2[] polygon, Rect2 rect)
	{
		if (polygon.Length == 0)
		{
			GD.PrintErr("Polygon has no vertices!");
			return new Vector2[0];
		}

		// Calculate the polygon's bounding box
		float minX = float.MaxValue;
		float maxX = float.MinValue;
		float minY = float.MaxValue;
		float maxY = float.MinValue;

		foreach (var point in polygon)
		{
			if (point.X < minX) minX = point.X;
			if (point.X > maxX) maxX = point.X;
			if (point.Y < minY) minY = point.Y;
			if (point.Y > maxY) maxY = point.Y;
		}

		Vector2 polygonSize = new Vector2(maxX - minX, maxY - minY);

		if (polygonSize.X == 0 || polygonSize.Y == 0)
		{
			GD.PrintErr("Polygon is degenerate!");
			return new Vector2[0];
		}

		// Calculate the scaling factor based on width (X-axis only)
		float scale = rect.Size.X / polygonSize.X;

		// Center of the polygon's bounding box
		Vector2 polygonCenter = new Vector2(minX + polygonSize.X / 2, minY + polygonSize.Y / 2);

		// Center of the target rectangle
		Vector2 rectCenter = rect.Position + rect.Size / 2;

		// Scale and translate each vertex
		Vector2[] scaledPolygon = new Vector2[polygon.Length];
		for (int i = 0; i < polygon.Length; i++)
		{
			var point = polygon[i];

			// Scale around the polygon's center
			Vector2 scaledPoint = polygonCenter + (point - polygonCenter) * scale;

			// Translate to the rectangle's center (adjust for Y-axis bounds)
			scaledPoint.Y += (rectCenter.Y - polygonCenter.Y) - (polygonSize.Y * scale) / 2;

			scaledPolygon[i] = scaledPoint;
		}

		return scaledPolygon;
	}


}
