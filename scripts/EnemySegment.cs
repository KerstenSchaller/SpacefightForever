using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class EnemySegment : Node2D
{

	Polygon2D polygon;

	public CollisionPolygon2D CollisionPoly2D { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		polygon = GetNode<Polygon2D>("Polygon2D");
	}


}
