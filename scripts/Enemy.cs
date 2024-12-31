using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 0.0f;

	public override void _Ready()
	{
		base._Ready();
		getCollisionShapesfromChildren(GetChildren());
	}

	void getCollisionShapesfromChildren(Godot.Collections.Array<Node> children)
	{
		foreach(var child in children)
		{
			if(child is EnemySegment)
			{
				//GD.Print("Adding child CollisionPolygon2D");
				var poly = ((EnemySegment)child).CollisionPolygon;
				child.RemoveChild(poly);
				AddChild(poly);
			}
			else
			{
				GD.Print("Not EnemySegment");
				getCollisionShapesfromChildren(child.GetChildren());
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{

		if(this.Position.X <= 0)this.Velocity = new Vector2(Speed,0);	
		if(this.Position.X >= 900)this.Velocity = new Vector2( -Speed,0);	


		//MoveAndSlide();
		MoveAndCollide(this.Velocity*(float)delta);
	}
}
