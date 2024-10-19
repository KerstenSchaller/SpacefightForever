using Godot;
using System;

[Tool]
public partial class ParentSizedPoly2d : Polygon2D
{
	CollisionPolygon2D parent;
	public override void _Ready()
	{
		parent = GetParent<CollisionPolygon2D>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 
		this.Polygon =  parent.Polygon;
	}
}
