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


	bool collided = false;
	public override void _PhysicsProcess(double delta)
	{
		if(collided == false)
		{
			var collision = MoveAndCollide(this.Velocity*(float)delta);
			//MoveAndSlide();

			if(collision != null && collided == false)
			{
				collided = true;
				// make explosion visible and limit object lifespan
				var bulletSprite = GetNode<Sprite2D>("bulletSprite");
				var explosionSprite = GetNode<Sprite2D>("5x5ExplosionSprite");

				bulletSprite.Visible = false;
				explosionSprite.Visible = true;
				aliveCounter = maxLifeTicks - 10;
			}
		}
		
		if(aliveCounter >= maxLifeTicks)
		{
			this.QueueFree();
		}
		aliveCounter++;
	}


}
