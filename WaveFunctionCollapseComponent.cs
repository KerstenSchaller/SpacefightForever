using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

class Bitfield
{
	byte data;


	public byte Data{get{return data;}}

	public bool compareBidirectional(Bitfield bitfieldToCompare)
	{
		// compare bitfields normally and with reversed bit order
		return (data == bitfieldToCompare.Data || ReversedData() == bitfieldToCompare.Data);
	}

	public bool compareBidirectional(byte dataToCompare)
	{
		// compare bitfields normally and with reversed bit order
		return (data == dataToCompare || ReversedData() == dataToCompare);
	}
	
	public bool compare(byte dataToCompare)
	{
		// compare bitfields normally and with reversed bit order
		return (data == dataToCompare);
	}

	public byte ReversedData()
	{
		byte value = data;
		byte result = 0;
		for (int i = 0; i < 8; i++)
		{
			result <<= 1;              		// Shift result to the left
			result |= (byte)(value & 1); // Add the least significant bit of value
			value >>= 1;               // Shift value to the right
		}
		return result;
	}

	public bool getBit(int position)
	{
		byte value = data;
		return ((value >> position) & 1) > 0 ;
	}

	
	public void setBit(int position, uint value)
	{
		if(value == 1)
		{
			// set bit
			data |= (byte)(1 << position);
		}
		else
		if(value == 0)
		{
			// clear bit
			data &= (byte)~(1 << position);
		}
		else{GD.Print("Invalid value for setBit(...)");}

		if(data == 8 && position == 8)
		{
			GD.Print("error");
		}

	}


	public string toString()
	{
		return Convert.ToString(data, 2).PadLeft(8, '0');
	}
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


	Bitfield sideUp = new Bitfield();
	Bitfield sideRight = new Bitfield();
	Bitfield sideDown = new Bitfield();
	Bitfield sideLeft = new Bitfield();


	public void setOrientationTypeLeft(){orientation = OrientationType.Left;}
	public void setOrientationTypeRight(){orientation = OrientationType.Right;}
	public void setOrientationTypeUp(){orientation = OrientationType.Up;}
	public void setOrientationTypeDown(){orientation = OrientationType.Down;}

	int xIndex;
	int yIndex;

	public Bitfield getIdSideUp()
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
		return new Bitfield();
	}

	public Bitfield getIdSideRight()
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
		return new Bitfield();
	}

	public Bitfield getIdSideDown()
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
		return new Bitfield();


	}

	public Bitfield getIdSideLeft()
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
		return new Bitfield();
	}




	public WFCTile(Texture2D texture, int _x, int _y)
	{
		xIndex = _x;
		yIndex = _y;
		texture2D = texture;
		try{
		var imageData = texture.GetImage().GetData();
		var imageDataSize = texture.GetSize();

		Image referncePalette = new Image();
		referncePalette.Load("2bit-demichrome-1x.png");
		var refColor = referncePalette.GetPixel(2, 0);

		
		for (int i = 0; i < 8; i++)
		{
			// first row
			bool condition1 = imageData[i*3] == refColor.R8 && imageData[i*3 + 1] == refColor.G8 && imageData[i*3 + 2] == refColor.B8;
			if (condition1) { sideUp.setBit(i, 1); } else { sideUp.setBit(i, 0); }

			// last column
			bool condition2 = imageData[i*24 + 21] == refColor.R8 && imageData[i*24 + 22] == refColor.G8 && imageData[i*24 + 23] == refColor.B8;
			if (condition2) { sideRight.setBit(i, 1); } else { sideRight.setBit(i, 0); }

			// last row, end to start
			bool condition3 = imageData[(7-i)*3+168] == refColor.R8 && imageData[(7-i)*3+169] == refColor.G8 && imageData[(7-i)*3+170] == refColor.B8;
			if (condition3) { sideDown.setBit(i, 1); } else { sideDown.setBit(i, 0); }

			// first column, bottom up
			bool condition = imageData[(7-i)*24] == refColor.R8 && imageData[(7-i)*24 + 1] == refColor.G8 && imageData[(7-i)*24 + 2] == refColor.B8;
			if (condition) { sideLeft.setBit(i, 1); } else { sideLeft.setBit(i, 0); }
		}
		}
		catch(Exception e)
		{
			GD.Print("error");
		}
	}
}



public partial class WaveFunctionCollapseComponent : Node2D
{
	//Texture2D atlasTexture = GD.Load<Texture2D>("SpaceshipSurfaceTilemap-export.png");
	Texture2D atlasTexture = GD.Load<Texture2D>("res://SpaceshipSurfaceTilemapReduced.png");
	//Texture2D atlasTexture = GD.Load<Texture2D>("res://2xTileset.png");

	List<WFCTile> WFCTiles = new List<WFCTile>();

	static int tilemapSize = 100;
	WFCTile[,] tileMap = new WFCTile[tilemapSize,tilemapSize]; 



	public override async void _Ready()
	{
		var atlasSize = atlasTexture.GetSize();
		// loop throuth all textures in texture atlas
		for(int y=0;y<atlasSize.Y/8;y++)
		{
			for(int x=0;x<atlasSize.X/8;x++)
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
		GD.Print("tilemap created");
		await CreatePNG();
	}



	WFCTile getMatchingTile(int x, int y)
	{
		byte upTileId = new byte();
		byte rightTileId = new byte();
		byte leftTileId = new byte();
		byte downTileId = new byte();

		bool useRightTile = true;
		bool useLeftTile = true;
		bool useUpTile = true;
		bool useDownTile = true;

		if (x > 0)
		{
			var leftTile = tileMap[y, x - 1];
			if (leftTile != null)
			{
				leftTileId = leftTile.getIdSideRight().Data;
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
				rightTileId = rightTile.getIdSideLeft().Data;
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
				upTileId = upTile.getIdSideDown().Data;
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
				downTileId = downTile.getIdSideUp().Data;
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
			if (useLeftTile && t.getIdSideLeft().compare(leftTileId) == false) continue;
			if (useDownTile && t.getIdSideDown().compare(downTileId) == false) continue;
			if (useRightTile && t.getIdSideRight().compare(rightTileId) == false) continue;
			if (useUpTile && t.getIdSideUp().compare(upTileId) == false) continue;
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





	async Task CreatePNG()
	{

		// Step 1: Create a viewport
		var viewport = new SubViewport();
		viewport.Size = new Vector2I(100*8, 100*8); // Set the size of the viewport
		//viewport.Usage = Viewport.UsageEnum.Usage2d; // Set to 2D mode
		viewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Once;
	
		
		// Add the viewport to the scene tree (required for rendering)
		AddChild(viewport);

		for (int y = 0; y < tilemapSize-2; y++)
		{
			for (int x = 0; x < tilemapSize-2; x++)
			{
				try
				{
					// Step 2: Add a sprite to the viewport
					var sprite = tileMap[y,x].getSprite();
					
					sprite.Position = new Vector2(x*8,y*8);
					viewport.AddChild(sprite);

				}
				catch(Exception e)
				{
					GD.Print("error3");
				}

			}
		}
		// Wait until the frame has finished before getting the texture.
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);

		// Step 3: Extract the rendered image
		var image = viewport.GetTexture().GetImage();
		//image.FlipY(); // Flip the image vertically for correct orientation

		// Step 4: Save the image as a PNG
		string savePath = "res://output_image.png";
		Error err = image.SavePng(savePath);

		// Step 5: Clean up resources
		viewport.QueueFree();


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
			Region = new Rect2(8*xIndex, 8*yIndex, 8, 8) // Specify the region (x, y, width, height)
		};
	}


}
