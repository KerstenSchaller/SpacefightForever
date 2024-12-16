using Godot;
using System;
using System.Collections.Generic;

public partial class Sprite2DSpawner : Node2D
{
    private Random _random = new Random();
    private List<Sprite2D> _spawnedSprite2Ds = new List<Sprite2D>(); // Member list to store spawned Sprite2Ds

    public List<Sprite2D> SpawnSprite2Ds(Sprite2D Sprite2D1, Sprite2D Sprite2D2, Sprite2D Sprite2D3, float prob1, float prob2, float prob3, int count1, int count2, int count3, Rect2 bounds)
    {
        if (prob1 + prob2 + prob3 != 1.0f)
        {
            GD.PrintErr("Probabilities must sum to 1.");
            return new List<Sprite2D>();
        }

        // Spawn Sprite2Ds based on their counts and probabilities
        SpawnSprite2DInstances(Sprite2D1, prob1, count1, bounds);
        SpawnSprite2DInstances(Sprite2D2, prob2, count2, bounds);
        SpawnSprite2DInstances(Sprite2D3, prob3, count3, bounds);
        return _spawnedSprite2Ds;
    }

    private void SpawnSprite2DInstances(Sprite2D Sprite2D, float probability, int count, Rect2 bounds)
    {
        for (int i = 0; i < count; i++)
        {
            // Check if this instance should be spawned based on its probability
            if (_random.NextDouble() <= probability)
            {
                var instance = (Sprite2D)Sprite2D.Duplicate(); // Duplicate the Sprite2D

                // Try to find a non-overlapping position
                Vector2 position;
                int maxAttempts = 100;
                bool validPosition = false;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    float x = (float)(_random.NextDouble() * bounds.Size.X + bounds.Position.X);
                    float y = (float)(_random.NextDouble() * bounds.Size.Y + bounds.Position.Y);
                    position = new Vector2(x, y);

                    if (IsPositionValid(position, instance, bounds))
                    {
                        validPosition = true;
                        instance.Position = position;
                        break;
                    }
                }

                if (!validPosition)
                {
                    //GD.PrintErr("Could not find a valid position for Sprite2D after max attempts.");
                    continue;
                }

                _spawnedSprite2Ds.Add(instance); // Add the Sprite2D to the member list
            }
        }
    }

    private bool IsPositionValid(Vector2 position, Sprite2D instance, Rect2 bounds)
    {
        // Check if the position is inside the bounds
        if (!bounds.HasPoint(position))
        {
            return false;
        }

        // Check for overlap with already spawned Sprite2Ds
        foreach (var Sprite2D in _spawnedSprite2Ds)
        {
            if (Sprite2D.GetRect().HasPoint(position))
            {
                return false;
            }
        }

        return true;
    }
}