using Godot;
using System;

public partial class CollisionPolygon2DWithID : CollisionPolygon2D
{
	
	int id = 0;
	static int lastId = 0;
	public CollisionPolygon2DWithID()
	{
		id = lastId+1;
		lastId = lastId+1;
	}

	public int getId(){return id;}



}
