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
	PackedScene bulletScene = GD.Load<PackedScene>("res://Bullet.tscn");

	public Vector2 shootingDirection;
	public float shootingFOVDeg;
	private Vector2 lastPosition;

	public override void _Ready()
	{
		shootingDirection = new Vector2(0,-1);
		lastPosition = this.Position;
		shootingFOVDeg = 20;
	}

	int speed = 300;

	private Vector2 getRandAngleinFOV()
	{
		
		var fovRad = shootingFOVDeg*Mathf.Pi/180;
		var shootingAngleDeg = Mathf.Atan2(shootingDirection.Y,shootingDirection.X);
		var v1 = shootingAngleDeg - fovRad/2;
		var v2 = shootingAngleDeg + fovRad/2;
		var min = Mathf.Min(v1,v2);
		var max = Mathf.Max(v1,v2);
		float randomFloat = (float)(new Random().NextDouble() * (max - min) + min);
		return new Vector2(Mathf.Cos(randomFloat),Mathf.Sin(randomFloat));
	}

	private void shootBullet()
	{
		Bullet bullet = bulletScene.Instantiate<Bullet>();
		bullet.Position = this.Position;
		bullet.setVelocity(getRandAngleinFOV());
		GetTree().Root.AddChild(bullet);
		
	}


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
			GD.Print("move1: " + displacement + ":" + shootingDirection + "," + angle + "," + movementInDirectionOrthogonal);
			shootingDirection = shootingDirection.Rotated(angle); 
			GD.Print("move2: " + shootingDirection);


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
		GD.Print("FOV: " + shootingFOVDeg);
		shootBullet();



		// Movement
		this.Velocity = new();
		if (Input.IsKeyPressed(Key.W)) //Up
		{
			this.Velocity = new Vector2( this.Velocity.X, -speed);
		}
		if (Input.IsKeyPressed(Key.S)) //down
		{
			this.Velocity = new Vector2( this.Velocity.X, speed);
		}
		if (Input.IsKeyPressed(Key.A)) //left
		{
			this.Velocity = new Vector2( -speed, this.Velocity.Y);
		}
		if (Input.IsKeyPressed(Key.D)) //right
		{
			this.Velocity = new Vector2( speed, this.Velocity.Y);
		}
		MoveAndSlide();


		QueueRedraw();


	}
}
