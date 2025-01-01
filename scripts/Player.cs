using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{

	OrbitWeaponHolder weapon;

	public override void _Ready()
	{
		weapon = GetNode<OrbitWeaponHolder>("OrbitWeaponHolder");
		DuplicateCollisionPolygons(this,Transform2D.Identity);
		//weapon.ShootingFOVDegree = shootingFOVDeg;
	}

	int speed = 300;

	Vector2 originPos;
	bool originSet = false;
	int circleRadius = 0;
	Vector2 circleCenter; 


	public override void _Draw()
	{
		if(circleRadius != 0)
		{
			DrawCircle(circleCenter - this.Position,circleRadius,Colors.DarkRed);
		}
	}

	public void DuplicateCollisionPolygons(Node parentNode, Transform2D accumulatedTransform)
	{
		foreach (Node child in parentNode.GetChildren())
		{
			if (child is CollisionPolygon2D collisionPolygon)
			{
				// Calculate the cumulative transform for this node
				Transform2D localTransform = collisionPolygon.Transform; // Local transform of this node
				Transform2D totalTransform = accumulatedTransform * localTransform;

				// Duplicate the CollisionPolygon2D
				CollisionPolygon2D duplicatedPolygon = (CollisionPolygon2D)collisionPolygon.Duplicate();

				// Apply the manually accumulated transform
				duplicatedPolygon.Position = totalTransform.Origin;
				duplicatedPolygon.Rotation = totalTransform.Rotation;

				// Add the duplicated node to the parent of the original
				this.AddChild(duplicatedPolygon);
			}
			else if (child is Node2D node2D)
			{
				// If the child is a Node2D, accumulate its transformation
				Transform2D localTransform = node2D.Transform; // Local transform of this Node2D
				Transform2D totalTransform = accumulatedTransform * localTransform;

				// Recurse into this child with the updated transform
				DuplicateCollisionPolygons(child, totalTransform);
			}
			else
			{
				// If it's not a Node2D or CollisionPolygon2D, simply recurse with the same transform
				DuplicateCollisionPolygons(child, accumulatedTransform);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		GameGlobal.PlayerPos = this.GlobalPosition;

		// Movement
		this.Velocity = new();
		if (Input.IsKeyPressed(Key.Up)) //Up
		{
			this.Velocity = new Vector2( this.Velocity.X, -speed);
		}
		if (Input.IsKeyPressed(Key.Down)) //down
		{
			this.Velocity = new Vector2( this.Velocity.X, speed);
		}
		if (Input.IsKeyPressed(Key.Left)) //left
		{
			this.Velocity = new Vector2( -speed, this.Velocity.Y);
		}
		if (Input.IsKeyPressed(Key.Right)) //right
		{
			this.Velocity = new Vector2( speed, this.Velocity.Y);
		}
		MoveAndCollide(this.Velocity*(float)delta);


		QueueRedraw();


	}
}
