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


	public Vector2 shootingDirection;
	public float shootingFOVDeg;
	private Vector2 lastPosition;

	WeaponComponent weapon;

	public override void _Ready()
	{
		weapon = GetNode<WeaponComponent>("WeaponComponent");

		shootingDirection = new Vector2(0,-1);
		lastPosition = this.Position;
		shootingFOVDeg = 20;

		weapon.ShootingFOVDegree = shootingFOVDeg;
		weapon.ShootingDirection = shootingDirection;
	}

	int speed = 300;






	Vector2 originPos;
	bool originSet = false;
	int circleRadius = 0;
	Vector2 circleCenter; 
	private void updateShootingFoVandAngle()
	{
		if (Input.IsKeyPressed(Key.Ctrl)) 
		{

			//movement since last frame
			Vector2 displacement = this.Position - lastPosition;

			// update shooting FOV (with movement in direction of shooting)
			float movementInDirection = displacement.Dot(shootingDirection.Normalized());
			

			shootingFOVDeg += movementInDirection*0.5f;
			shootingFOVDeg = Mathf.Clamp(shootingFOVDeg,2.5f,70);


			// update shooting direction(with movement orthogonal to direction of shooting)
			float movementInDirectionOrthogonal = displacement.Dot(shootingDirection.Rotated(-90).Normalized());
			var angle = 0.5f*movementInDirectionOrthogonal*(Mathf.Pi/180);
			shootingDirection = shootingDirection.Rotated(angle); 

			weapon.ShootingFOVDegree = shootingFOVDeg;
			weapon.ShootingDirection = shootingDirection;

		}
		lastPosition = this.Position;
	}

	public override void _Draw()
	{
		if(circleRadius != 0)
		{
			DrawCircle(circleCenter - this.Position,circleRadius,Colors.DarkRed);
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		//shooting
		updateShootingFoVandAngle();



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
