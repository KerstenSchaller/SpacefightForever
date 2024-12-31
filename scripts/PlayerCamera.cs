using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	OrbitWeaponHolder orbitWeaponHolder;

	[Export(PropertyHint.Range, "-1190,0,5")]
	public float Distance{get;set;}
	public override void _Ready()
	{
		orbitWeaponHolder = GetNode<OrbitWeaponHolder>("../OrbitWeaponHolder");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var dir = orbitWeaponHolder.shootingDirection;
		//this.Position = -1*dir*Distance;
	}
}
