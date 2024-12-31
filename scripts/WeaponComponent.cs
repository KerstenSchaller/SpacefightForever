using Godot;
using System;

public partial class WeaponComponent : Node2D
{
	Vector2 shootingDirection = new Vector2(0,-1);
	float shootingFOVDegree = 0;
	Node2D spawnPoint;

	
	public Vector2 ShootingDirection
	{
		get { return shootingDirection; }
		
		set
		{
			shootingDirection = value;
			//GD.Print("set shootingDirection " + value );
		}
		
	}
	

	public float ShootingFOVDegree
	{
		get { return shootingFOVDegree; }
		set
		{
			shootingFOVDegree = value;
			//GD.Print("set shootingFOVDegree " + value );
		}
	}

	Random randComponent = new Random();
	

	PackedScene bulletScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn");

	public override void _Ready()
	{
		spawnPoint = GetNode<Node2D>("SpawnPoint");
	}

	public override void _Process(double delta)
	{
	}
	

	uint cnt;
	public override void _PhysicsProcess(double delta)
	{
		//if(cnt == 60)
		{

			shootBullet();
			cnt=0;
		}
		cnt++;
	}

	private void shootBullet()
	{
		Projectile bullet = bulletScene.Instantiate<Projectile>();
		bullet.setVelocity(getRandAngleinFOV());
		bullet.Position = ToGlobal(spawnPoint.Position);
		
		//AddChild(bullet);
		GetTree().Root.AddChild(bullet);
		
	}

	private Vector2 getRandAngleinFOV()
	{

		var fovRad = shootingFOVDegree * Mathf.Pi / 180;
		//var shootingAngleDeg = Mathf.Atan2(shootingDirection.Y, shootingDirection.X);
		var shootingAngleDeg = Rotation - Mathf.Pi/2;
		var v1 = shootingAngleDeg - fovRad / 2;
		var v2 = shootingAngleDeg + fovRad / 2;
		var min = Mathf.Min(v1, v2);
		var max = Mathf.Max(v1, v2);
		var nd = randComponent.NextDouble();
		
		float randomFloat = (float)(nd * (max - min) + min);
		return new Vector2(Mathf.Cos(randomFloat), Mathf.Sin(randomFloat));
	}
}
