using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class EnemySegment : CharacterBody2D
{
	float Health = 250;

	public void takeDamage(float damage)
	{
		Health -= damage;
		checkHealth();
	}

	void checkHealth()
	{
		if(Health <= 0)
		{
			this.QueueFree();
		}
	}

	public override void _Ready()
	{
		
	}




	public override void _Process(double delta)
	{
	}
}



