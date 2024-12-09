using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;




struct Pixel
{
	byte B8;
	byte G8;
	byte R8;

	public Pixel(byte _R8,byte _G8,byte _B8){R8 = _R8;G8=_G8;B8=_B8;}
	public static bool operator ==(Pixel a, Pixel b){return a.R8 == b.R8 && a.G8 == b.G8 && a.B8 == b.B8;}
	public static bool operator !=(Pixel a, Pixel b){return !(a == b);}
}

class WFCTile
{
	public Texture2D texture2D;
	public Sprite2D getSprite()
	{

		Sprite2D sprite = new Sprite2D();
		sprite.Texture = texture2D;
		switch (orientation)
		{
			case OrientationType.Down:
				sprite.Rotate(MathF.PI);
				break;
			case OrientationType.Left:
				sprite.Rotate(-MathF.PI / 2);
				break;
			case OrientationType.Right:
				sprite.Rotate(MathF.PI/2);
				break;
		}
		return sprite;
	}

	public enum OrientationType{Up,Down,Left,Right};
	public OrientationType orientation = OrientationType.Up;


	List<Pixel> sideUp = new List<Pixel>();
	List<Pixel> sideRight = new List<Pixel>();
	List<Pixel> sideDown = new List<Pixel>();
	List<Pixel> sideLeft = new List<Pixel>();


	public void setOrientationTypeLeft(){orientation = OrientationType.Left;}
	public void setOrientationTypeRight(){orientation = OrientationType.Right;}
	public void setOrientationTypeUp(){orientation = OrientationType.Up;}
	public void setOrientationTypeDown(){orientation = OrientationType.Down;}

	int xIndex;
	int yIndex;

	public List<Pixel> getSideUp()
	{
		switch(orientation)
		{
			case OrientationType.Up:
				return sideUp;
			case OrientationType.Right:
				return sideLeft;
			case OrientationType.Down:
				return sideDown;
			case OrientationType.Left:
				return sideRight;
		}
		return new List<Pixel>();
	}

	public List<Pixel> getSideRight()
	{
		switch(orientation)
		{
			case OrientationType.Up:
				return sideRight;
			case OrientationType.Right:
				return sideUp;
			case OrientationType.Down:
				return sideLeft;
			case OrientationType.Left:
				return sideDown;
		}
		return new List<Pixel>();
	}

	public List<Pixel> getSideDown()
	{
		switch(orientation)
		{
			case OrientationType.Up:
				return sideDown;
			case OrientationType.Right:
				return sideRight;
			case OrientationType.Down:
				return sideUp;
			case OrientationType.Left:
				return sideLeft;
		}
		return new List<Pixel>();


	}

	public List<Pixel> getSideLeft()
	{
		switch(orientation)
		{
			case OrientationType.Up:
				return sideLeft;
			case OrientationType.Right:
				return sideDown;
			case OrientationType.Down:
				return sideRight;
			case OrientationType.Left:
				return sideUp;
		}
		return new List<Pixel>();
	}

	void ProcessImageBorder(byte[] imageData, int width, int height)
	{
		if (imageData == null || imageData.Length != width * height * 3)
			throw new ArgumentException("Invalid pixel array or dimensions.");

		// Top edge: Left to right
		for (int x = 0; x < width; x++)
		{
			int index = x * 3; // Row 0, column x
			sideUp.Add(new Pixel(imageData[index],imageData[index+1],imageData[index+2]));
		}

		// Right edge: Top to bottom
		for (int y = 0; y < height; y++)
		{
			int index = (y * width + (width - 1)) * 3; // Row y, last column
			sideRight.Add(new Pixel(imageData[index],imageData[index+1],imageData[index+2]));
		}

		// Bottom edge: Right to left
		for (int x = width - 1; x >= 0; x--)
		{
			int index = ((height - 1) * width + x) * 3; // Last row, column x
			sideDown.Add(new Pixel(imageData[index],imageData[index+1],imageData[index+2]));
		}

		// Left edge: Bottom to top
		for (int y = height - 1; y >= 0; y--)
		{
			int index = (y * width) * 3; // Row y, first column
			sideLeft.Add(new Pixel(imageData[index],imageData[index+1],imageData[index+2]));
		}
	}

	public WFCTile(Texture2D texture, int _x, int _y)
	{
		xIndex = _x;
		yIndex = _y;
		texture2D = texture;
		try{
		var imageData = texture.GetImage().GetData();
		var imageDataSize = texture.GetSize();



		int width = (int)imageDataSize.X;
		int height = (int)imageDataSize.Y;

		ProcessImageBorder(imageData,width,height);
		}
		catch(Exception e)
		{
			GD.Print("error2 " + e.ToString());
		}
	}
}



public partial class WaveFunctionCollapseComponent : Node2D
{
	Texture2D atlasTexture = GD.Load<Texture2D>("res://tilemaps/2ColorSpaceshipTilemap_8x8.png");
	//Texture2D atlasTexture = GD.Load<Texture2D>("res://SpaceshipSurfaceTilemapReduced.png");
	//Texture2D atlasTexture = GD.Load<Texture2D>("res://tilemaps/FCWTilemap_Ink1X.png");
	//Texture2D atlasTexture = GD.Load<Texture2D>("res://2xTileset.png");

	List<WFCTile> WFCTiles = new List<WFCTile>();

	WFCTile[,] tileMap = new WFCTile[tilemapSize,tilemapSize]; 
	static int tilemapSize = 100;
	static int BitSize = 8;

	PNGCreator pNGCreator;

	Texture2D outputTexture;
	public Texture2D getOutputTexture()
	{
		return pNGCreator.getOutputTexture();
	}

	public override void _Ready()
	{
		pNGCreator = new PNGCreator();
		AddChild(pNGCreator);

		var atlasSize = atlasTexture.GetSize();
		// loop throuth all textures in texture atlas
		for(int y=0;y<atlasSize.Y/BitSize;y++)
		{
			for(int x=0;x<atlasSize.X/BitSize;x++)
			{
				var texture = getTexture(x,y);
				WFCTile wFCTile = new WFCTile(texture,x,y);
				wFCTile.setOrientationTypeUp();
				WFCTiles.Add(wFCTile);


				WFCTile wFCTile1 = new WFCTile(texture,x,y);
				wFCTile1.setOrientationTypeDown();
				WFCTiles.Add(wFCTile1);

				WFCTile wFCTile2 = new WFCTile(texture,x,y);
				wFCTile2.setOrientationTypeLeft();
				WFCTiles.Add(wFCTile2);

				WFCTile wFCTile3 = new WFCTile(texture,x,y);
				wFCTile3.setOrientationTypeRight();
				WFCTiles.Add(wFCTile3);

			}
		}

		// tesselation
		var numberOfTiles = WFCTiles.Count;
		var randIndex = new Random().Next(numberOfTiles);
		var tile = WFCTiles[randIndex];
		tileMap[0,0] = tile; 

		for (int y = 0; y < tilemapSize; y++)
		{
			for (int x = 0; x < tilemapSize; x++)
			{
				if(x == 0 && y == 0)continue;
				var matchingTile = getMatchingTile(x,y);
				tileMap[y,x] = matchingTile;
			}
		}
	
		// prepare sprites for drawing
		List<Sprite2D> sprites = new List<Sprite2D>();
		for (int y = 0; y < tilemapSize-2; y++)
		{
			for (int x = 0; x < tilemapSize - 2; x++)
			{

				// Step 2: Add a sprite to the viewport
				var sprite = tileMap[y, x].getSprite();

				sprite.Position = new Vector2(x * BitSize, y * BitSize);
				sprites.Add(sprite);
			}
		}

		
		PNGCreator.Config config;
		config.sizePixels = new Vector2I(100*BitSize, 100*BitSize);
		config.savePath = "res://output_image.png";
		config.sprites = sprites;

		pNGCreator.config = config;
		pNGCreator.run(config);


	}

	bool compareSide(List<Pixel> side1,List<Pixel> side2)
	{
		for(int i = 0;i<side1.Count;i++)
		{
			if(side1[i] != side2[i])return false;
		}
		return true;
	}

	WFCTile getMatchingTile(int x, int y)
	{
		List<Pixel> upTileId = new List<Pixel>();
		List<Pixel> rightTileId = new List<Pixel>();
		List<Pixel> leftTileId = new List<Pixel>();
		List<Pixel> downTileId = new List<Pixel>();

		bool useRightTile = true;
		bool useLeftTile = true;
		bool useUpTile = true;
		bool useDownTile = true;

		if (x > 0)
		{
			var leftTile = tileMap[y, x - 1];
			if (leftTile != null)
			{
				leftTileId = leftTile.getSideRight();
			}
			else
			{
				useLeftTile = false;
			}

		}
		else { useLeftTile = false; }
		if (x < tilemapSize - 1)
		{
			var rightTile = tileMap[y, x + 1];



			if (rightTile != null)
			{
				rightTileId = rightTile.getSideLeft();
			}
			else
			{
				useRightTile = false;
			}

		}
		else { useRightTile = false; }
		if (y > 0)
		{
			var upTile = tileMap[y - 1, x];
			if (upTile != null)
			{
				upTileId = upTile.getSideDown();
			}
			else
			{
				useUpTile = false;
			}
		}
		else { useUpTile = false; }
		if (y < tilemapSize - 1)
		{
			var downTile = tileMap[y + 1, x];

			if (downTile != null)
			{
				downTileId = downTile.getSideUp();
			}
			else
			{
				useDownTile = false;
			}
		}
		else { useDownTile = false; }


		List<WFCTile> tiles = new List<WFCTile>();
		foreach (var t in WFCTiles)
		{
			if (useLeftTile && compareSide(leftTileId, t.getSideLeft()) == false) continue;
			if (useDownTile && compareSide(downTileId, t.getSideDown()) == false) continue;
			if (useRightTile && compareSide(rightTileId, t.getSideRight()) == false) continue;
			if (useUpTile && compareSide(upTileId, t.getSideUp()) == false) continue;
			tiles.Add(t);
		}
		int randomIndex = new Random().Next(tiles.Count);

		if (tiles.Count == 0)
		{
			GD.Print("Error, tiles count = 0... propably some combinations of sides are non existant in the tileset");
		}


		var retTile = tiles[randomIndex];
		return retTile;
	}








	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//QueueRedraw();
	}

	
	
	AtlasTexture getTexture(int xIndex, int yIndex)
	{
		// Create a new AtlasTexture instance
		return new AtlasTexture
		{
			Atlas = atlasTexture, // Assign the atlas
			Region = new Rect2(BitSize*xIndex, BitSize*yIndex, BitSize, BitSize) // Specify the region (x, y, width, height)
		};
	}


}
