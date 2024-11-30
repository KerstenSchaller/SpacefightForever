using Godot;
using System;
public partial class FOVLine : Line2D
{
	private bool inverted = false;

	[Export]
	public bool Inverted
	{
		get => inverted;
		set => inverted = value;
	}

	[Export(PropertyHint.Range, "100,600,5")]
	private static float length = 300;

	[Export]
	public float Length
	{
		get => length;
		set => length = value;
	}

	
	public override void _Ready()
	{
		Width = 1;
		DefaultColor = Colors.Yellow;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		var parent = GetParent<WeaponComponent>();
		Vector2 shootingDirection = parent.shootingDirection.Normalized();
		float shootingFOV = parent.shootingFOVDegree/2*(Mathf.Pi/180);
		if(Inverted)
		{
			shootingFOV = 2*Mathf.Pi-shootingFOV;
		}

		Vector2 p2 = shootingDirection.Rotated(shootingFOV)*length;


		Points = new Vector2[]{new Vector2(),p2};
		QueueRedraw();
	}
}
