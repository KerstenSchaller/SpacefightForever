using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 0.0f;



	public override void _PhysicsProcess(double delta)
	{

		if(this.Position.X <= 0)this.Velocity = new Vector2(Speed,0);	
		if(this.Position.X >= 900)this.Velocity = new Vector2( -Speed,0);	


		MoveAndSlide();
	}
}
