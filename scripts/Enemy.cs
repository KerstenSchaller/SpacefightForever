using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 0.0f;

	Dictionary<int,CollisionPolygon2DWithID> childPolygons = new Dictionary<int, CollisionPolygon2DWithID>();

	public override void _Ready()
	{
		base._Ready();
		DuplicateCollisionPolygons(this,Transform2D.Identity);
	}

	Vector2 currentTransform = new Vector2();
	float currentRotation = 0;


	// Recurses into children, finds collisionPolygons, then applies each parents transformation 
	// so that the duplicate appears in the same position when added as a child to this
	public void DuplicateCollisionPolygons(Node parentNode, Transform2D accumulatedTransform)
	{
		foreach (Node child in parentNode.GetChildren())
		{
			if (child is CollisionPolygon2DWithID collisionPolygon)
			{
				// Calculate the cumulative transform for this node
				Transform2D localTransform = collisionPolygon.Transform; // Local transform of this node
				Transform2D totalTransform = accumulatedTransform * localTransform;

				// Duplicate the CollisionPolygon2D
				CollisionPolygon2DWithID duplicatedPolygon = (CollisionPolygon2DWithID)collisionPolygon.Duplicate();

				// Apply the manually accumulated transform
				duplicatedPolygon.Position = totalTransform.Origin;
				duplicatedPolygon.Rotation = totalTransform.Rotation;

				// Add the duplicated node to the parent of the original
				this.AddChild(duplicatedPolygon);
				childPolygons.Add(collisionPolygon.getId(),duplicatedPolygon);
			}
			else if (child is Node2D node2D)
			{
				// If the child is a Node2D, accumulate its transformation
				Transform2D localTransform = node2D.Transform; // Local transform of this Node2D
				Transform2D totalTransform = accumulatedTransform * localTransform;

				// Recurse into this child with the updated transform
				DuplicateCollisionPolygons(child, totalTransform);
			}
			else
			{
				// If it's not a Node2D or CollisionPolygon2D, simply recurse with the same transform
				DuplicateCollisionPolygons(child, accumulatedTransform);
			}
		}
	}

	public Vector2 RotatePoint(Vector2 point, Vector2 origin, float angle)
	{
		// Translate the point to the origin
		Vector2 translatedPoint = point - origin;

		// Perform the rotation
		float sinAngle = Mathf.Sin(angle);
		float cosAngle = Mathf.Cos(angle);

		float rotatedX = translatedPoint.X * cosAngle - translatedPoint.Y * sinAngle;
		float rotatedY = translatedPoint.X * sinAngle + translatedPoint.Y * cosAngle;

		// Translate the point back to its original position
		Vector2 rotatedPoint = new Vector2(rotatedX, rotatedY) + origin;

		return rotatedPoint;
	}

	public override void _PhysicsProcess(double delta)
	{

		if(this.Position.X <= 0)this.Velocity = new Vector2(Speed,0);	
		if(this.Position.X >= 900)this.Velocity = new Vector2( -Speed,0);	


		//MoveAndSlide();
		//MoveAndCollide(this.Velocity*(float)delta);
	}

	public void removeCollisionPoly(int id)
	{
		if(childPolygons.ContainsKey(id))
		{
			childPolygons[id].QueueFree();
			childPolygons.Remove(id);

		}
	}

}
