using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class EnemySegment : Node2D
{
	float Health = 250;

	public CollisionPolygon2D CollisionPolygon {get;private set;}

	public void takeDamage(float damage)
	{
		Health -= damage;
		checkHealth();
	}

	void checkHealth()
	{
		if(Health <= 0)
		{
			this.QueueFree();
		}
	}

	public override void _Ready()
	{
		CollisionPolygon = GetNode<CollisionPolygon2D>("CollisionPolygon2D");	

		if (!Engine.IsEditorHint())
		{
			CollisionPolygon.Position = this.Position;	
			CollisionPolygon.Rotation = this.Rotation;
		}


	}




	public override void _Process(double delta)
	{
	}
}



