using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{

/*
	[Export(PropertyHint.Range, "0,90,5")]
	public float ShootingFOV
	{
		get => shootingFOVDeg;
		set => shootingFOVDeg = value;
	}
*/




	OrbitWeaponHolder weapon;

	public override void _Ready()
	{
		weapon = GetNode<OrbitWeaponHolder>("OrbitWeaponHolder");



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
		MoveAndSlide();


		QueueRedraw();


	}
}
