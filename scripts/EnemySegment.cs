using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class EnemySegment : StaticBody2D, IDamagable
{
	public CharacterBody2D parentCharachterBody;

	[Export]
	public Polygon2D polygon2D { get; set; }

	[Export]
	public bool UsePolyHelper { get; set; }
	
	[Export]
	public int NumberOfVertices { get; set; }

	[Export(PropertyHint.Range, "0,150,0.1")]
	public float SideLength { get; set; }

	float Health = 250;

	CollisionPolygon2DWithID collisionPolygon2D;
	CollisionPolygon2D copyPoly;

	public void takeDamage(float damage, int index)
	{
		Health -= damage;
		checkHealth();
	}

	void checkHealth()
	{
		if(Health <= 0)
		{
			((Enemy)parentCharachterBody).removeCollisionPoly(collisionPolygon2D.getId());
			this.QueueFree();
		}
	}

	int breakCounter = 0;

	public Vector2 ParentRelativePosition{get;private set;}
	private CharacterBody2D getCharacterBody2D(Node node)
	{
		if(breakCounter >= 10)
		{
			GD.PrintErr("EnemySegment: No parent charachterbody found!");
			return null;
		}
		if(node is CharacterBody2D)
		{
			GD.Print("Found parent after " + breakCounter + " iterations");
			return (CharacterBody2D)node;
		}
		else
		{
			breakCounter++;
			return getCharacterBody2D(node.GetParent());
		}
	}

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			collisionPolygon2D = GetNode<CollisionPolygon2DWithID>("CollisionPolygon2D");
			parentCharachterBody = getCharacterBody2D(this);
			return;

		}


	}




	public override void _Process(double delta)
	{
	}
}



