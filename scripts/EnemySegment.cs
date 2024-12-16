using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class EnemySegment : Area2D
{
	public override void _Ready()
	{
		// Connect the 'body_entered' signal to a method
	}

	private void _on_body_entered(Node body)
	{
		if (body is CharacterBody2D)
		{
			GD.Print("KinematicBody2D entered the Area2D");
			// You can cast to KinematicBody2D for more specific operations
			CharacterBody2D characterBody2D = (CharacterBody2D)body;
		}
	}

	private void _on_body_exited(Node body)
	{
		if (body is CharacterBody2D)
		{
			GD.Print("KinematicBody2D exited the Area2D");
		}
	}



	public override void _Process(double delta)
	{
		
	}
}



