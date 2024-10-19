using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	Vector2 direction;
	float maxSpeed = 500;

	int aliveCounter = 0;
	int maxLifeTicks = 3*60;

	public void setVelocity(Vector2 direction)
	{
		this.Velocity = direction.Normalized()*maxSpeed;
		this.Rotate(Mathf.Atan2(direction.Y,direction.X)-Mathf.Pi/2);
	}

	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(this.Velocity*(float)delta);
		//MoveAndSlide();

		if(collision != null)
		{
			//if (((Node)collision.GetCollider()).IsInGroup("ball"))
			//{
			this.QueueFree();
		}

		aliveCounter++;
		if(aliveCounter >= maxLifeTicks)
		{
			this.QueueFree();
		}
	}


}
