using Godot;
using System;

[Tool]
public partial class EnemySegment : Node2D
{

	private CollisionPolygon2D _collisionPoly2D;

	[Export]
	public CollisionPolygon2D CollisionPoly2D { get; set; }




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		CollisionPoly2D.Position = this.Position;
		QueueRedraw();
	}

	public override void _Draw()
	{

		// create a color array and vector array flipped according to scale
		Color[] colors = new Color[CollisionPoly2D.Polygon.Length];
		Vector2[] poly = new Vector2[CollisionPoly2D.Polygon.Length];
		for(int i = 0;i<colors.Length;i++)
		{
			colors[i] = Colors.SeaGreen;
			//poly[i] = CollisionPoly2D.Polygon[i]*CollisionPoly2D.Scale;
			poly[i] = new Vector2(CollisionPoly2D.Polygon[i].X*CollisionPoly2D.Scale[1],CollisionPoly2D.Polygon[i].Y*CollisionPoly2D.Scale[0]);
		}

		DrawPolygon(poly,colors);
		base._Draw();
	}
}
