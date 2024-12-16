using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

public partial class BackGround : Node2D
{
	Texture2D atlas;
	PNGCreator pNGCreator;
	bool loaded;
	TextureRect backGroundTextureRect1;
	TextureRect backGroundTextureRect2;
	TextureRect backGroundTextureRect3;
	TextureRect backGroundTextureRect4;
	TextureRect backGroundTextureRect5;
	TextureRect backGroundTextureRect6;
	TextureRect backGroundTextureRect7;
	TextureRect backGroundTextureRect8;
	TextureRect backGroundTextureRect9;



	public override void _Ready()
	{



		atlas = GD.Load<Texture2D>("res://assets/Stars.png");
		
		backGroundTextureRect1 = new TextureRect();
		backGroundTextureRect2 = new TextureRect();
		backGroundTextureRect3 = new TextureRect();
		backGroundTextureRect4 = new TextureRect();
		backGroundTextureRect5 = new TextureRect();
		backGroundTextureRect6 = new TextureRect();
		backGroundTextureRect7 = new TextureRect();
		backGroundTextureRect8 = new TextureRect();
		backGroundTextureRect9 = new TextureRect();


		pNGCreator = new PNGCreator();
		AddChild(pNGCreator);

		int sideLength = 1000;

		PNGCreator.Config config;
		config.savePath = "backGround.png";
		config.sizePixels = new Vector2I(sideLength,sideLength);

		List<Sprite2D> sprites = new List<Sprite2D>();
		TextureRect textureRect = new TextureRect();
		Sprite2D bg = getSprite(3,0);
		bg.Scale = config.sizePixels;
		
		bg.Modulate = GameColor.Color0;
		sprites.Add(bg);
	 	var s1 = getSprite(0,0);
	 	var s2 = getSprite(1,0);
	 	var s3 = getSprite(2,0);

		Rect2 rect2 = new Rect2(0,0,sideLength,sideLength);
		Sprite2DSpawner sprite2DSpawner = new Sprite2DSpawner();

		float propSprite1 = 0.6f;
		float propSprite2 = 0.3f;
		float propSprite3 = 0.1f;
		int numberSprite1 = 200;
		int numberSprite2 = 50;
		int numberSprite3 = 25;

		var scatterSprites = sprite2DSpawner.SpawnSprite2Ds(s1,
															s2,
															s3,
															propSprite1,
															propSprite2,
															propSprite3,
															numberSprite1,
															numberSprite2,
															numberSprite3,
															rect2);

		foreach (var s in scatterSprites)
		{
			sprites.Add(s);
		}

		sprites.Add(s1);
		sprites.Add(s2);
		sprites.Add(s3);
		config.sprites = sprites;
		pNGCreator.config = config;
		pNGCreator.run(); 
	}

	Sprite2D getSprite(int xIndex, int yIndex)
	{
		var at = new AtlasTexture
		{
			Atlas = atlas, // Assign the atlas
			Region = new Rect2(8*xIndex, 8*yIndex, 8, 8) // Specify the region (x, y, width, height)
		};
		Sprite2D sprite2D = new Sprite2D();
		sprite2D.Texture = at;
		return sprite2D;
	}

	public override void _Process(double delta)
	{
		if(loaded == false)
		{
			var tex = pNGCreator.getOutputTexture();
			if(tex != null)
			{
				backGroundTextureRect1.Texture = tex;
				backGroundTextureRect2.Texture = tex;
				backGroundTextureRect3.Texture = tex;
				backGroundTextureRect4.Texture = tex;
				backGroundTextureRect5.Texture = tex;
				backGroundTextureRect6.Texture = tex;
				backGroundTextureRect7.Texture = tex;
				backGroundTextureRect8.Texture = tex;
				backGroundTextureRect9.Texture = tex;

				backGroundTextureRect1.ZIndex = -5;
				backGroundTextureRect2.ZIndex = -5;
				backGroundTextureRect3.ZIndex = -5;
				backGroundTextureRect4.ZIndex = -5;
				backGroundTextureRect5.ZIndex = -5;
				backGroundTextureRect6.ZIndex = -5;
				backGroundTextureRect7.ZIndex = -5;
				backGroundTextureRect8.ZIndex = -5;
				backGroundTextureRect9.ZIndex = -5;


				GetParent().AddChild(backGroundTextureRect1);
				GetParent().AddChild(backGroundTextureRect2);
				GetParent().AddChild(backGroundTextureRect3);
				GetParent().AddChild(backGroundTextureRect4);
				GetParent().AddChild(backGroundTextureRect5);
				GetParent().AddChild(backGroundTextureRect6);
				GetParent().AddChild(backGroundTextureRect7);
				GetParent().AddChild(backGroundTextureRect8);
				GetParent().AddChild(backGroundTextureRect9);


				loaded = true;
				
			}
		}
		var grid = GetGridCellAndNeighbours(GameGlobal.PlayerPos);
		backGroundTextureRect1.Position = grid[0];
		backGroundTextureRect2.Position = grid[1];
		backGroundTextureRect3.Position = grid[2];
		backGroundTextureRect4.Position = grid[3];
		backGroundTextureRect5.Position = grid[4];
		backGroundTextureRect6.Position = grid[5];
		backGroundTextureRect7.Position = grid[6];
		backGroundTextureRect8.Position = grid[7];
		backGroundTextureRect9.Position = grid[8];
		QueueRedraw();
	}

	public List<Vector2> GetGridCellAndNeighbours(Vector2 position)
	{
		Vector2 _cellSize = new Vector2(1000, 1000);

		// Calculate the grid cell corresponding to the given position
		Vector2 centerCell = new Vector2(
			Mathf.Floor(position.X / _cellSize.X),
			Mathf.Floor(position.Y / _cellSize.Y)
		);

		// Define the 8 possible neighbours relative to the center cell
		Vector2[] neighbourOffsets = {
			new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1), // Top row
			new Vector2(-1,  0),                     new Vector2(1,  0), // Middle row (excluding center)
			new Vector2(-1,  1), new Vector2(0,  1), new Vector2(1,  1)  // Bottom row
		};

		// Add the center cell and the neighbouring cells to the result list
		List<Vector2> result = new List<Vector2> { centerCell }; // Start with the center cell
		foreach (var offset in neighbourOffsets)
		{
			result.Add(new Vector2((centerCell.X + offset.X)*_cellSize.X, (centerCell.Y + offset.Y)*_cellSize.Y));
		}
		result[0] = new Vector2(centerCell.X*_cellSize.X,centerCell.Y*_cellSize.Y);

		return result;
	}

}
