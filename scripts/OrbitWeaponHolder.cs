using Godot;
using System;

[Tool]
public partial class OrbitWeaponHolder : Node2D
{
	[Export(PropertyHint.Range, "3,50,0.2")]
	float _Radius{get;set;}

	Color _Color = GameColor.Color3;

	public Vector2 shootingDirection;
	float shootingFOVDeg;
	private Vector2 lastGlobalPosition;


	WeaponComponent weaponComponent;

	public override void _Ready()
	{
		shootingDirection = new Vector2(0,-1);
		lastGlobalPosition = ToGlobal(this.Position);
		shootingFOVDeg = 20;

		if (!Engine.IsEditorHint())
		{
			weaponComponent = GetNode<WeaponComponent>("WeaponComponent");
		}
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		updateShootingFoVandAngle();

		//update weapon
		if(weaponComponent != null)
		{
			weaponComponent.Position = _Radius*shootingDirection;
			weaponComponent.Rotation = Mathf.Atan2(shootingDirection.Y,shootingDirection.X) + MathF.PI/2;
			weaponComponent.ShootingFOVDegree = shootingFOVDeg;
		}
	}


	public override void _Draw()
	{
		//DrawCircle(new Vector2(), _Radius, Colors.Red);
		DrawArc(new Vector2(),_Radius,0,2*Mathf.Pi,360,_Color,1);
		base._Draw();
	}

	private void updateShootingFoVandAngle()
	{
		if (Input.IsKeyPressed(Key.Ctrl)) 
		{

			//movement since last frame
			Vector2 displacement = ToGlobal(this.Position) - lastGlobalPosition;

			// update shooting FOV (with movement in direction of shooting)
			float movementInDirection = displacement.Dot(shootingDirection.Normalized());
			

			shootingFOVDeg += movementInDirection*0.5f;
			shootingFOVDeg = Mathf.Clamp(shootingFOVDeg,2.5f,70);


			// update shooting direction(with movement orthogonal to direction of shooting)
			float movementInDirectionOrthogonal = displacement.Dot(shootingDirection.Rotated(-90).Normalized());
			var angle = 0.5f*movementInDirectionOrthogonal*(Mathf.Pi/180);
			shootingDirection = shootingDirection.Rotated(angle); 

			//weapon.ShootingFOVDegree = shootingFOVDeg;


		}
		lastGlobalPosition = ToGlobal(this.Position);
	}
}
