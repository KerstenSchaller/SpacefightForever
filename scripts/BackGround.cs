using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

public partial class BackGround : Node2D
{
	Texture2D atlas = GD.Load<Texture2D>("res://assets/Stars.png");
	PNGCreator pNGCreator;
	bool loaded;
	TextureRect backGroundTextureRect;

	public override void _Ready()
	{
		backGroundTextureRect = new TextureRect();
		AddChild(backGroundTextureRect);

		pNGCreator = new PNGCreator();
		AddChild(pNGCreator);


		//atlas = GetNode<AtlasTexture>("Atlas");

		PNGCreator.Config config;
		config.savePath = "backGround.png";
		config.sizePixels = new Vector2I(50,50);

		List<Sprite2D> sprites = new List<Sprite2D>();
		TextureRect textureRect = new TextureRect();
		Sprite2D bg = getSprite(3,0);
		bg.Scale = new Vector2(1000,1000);
		
		bg.Modulate = GameColor.Color0;
		sprites.Add(bg);
	 	var s1 = getSprite(0,0);
	 	var s2 = getSprite(1,0);
	 	var s3 = getSprite(2,0);
		s1.Position = new Vector2(5,5);
		s2.Position = new Vector2(15,15);
		s3.Position = new Vector2(25,25);
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
			Region = new Rect2(5*xIndex, 5*yIndex, 5, 5) // Specify the region (x, y, width, height)
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
				backGroundTextureRect.Texture = tex;

				loaded = true;
				QueueRedraw();
			}
		}
	}
}
