using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;

public partial class PNGCreator : Node2D
{
	public struct Config
	{
		public Vector2I sizePixels;
		public string savePath;
		public List<Sprite2D> sprites;
	}

	public Config config;

	Texture2D outputTexture;
	public Texture2D getOutputTexture(){return outputTexture;}

	
	public async void CreatePNG()
	{

		// Step 1: Create a viewport
		var viewport = new SubViewport();
		viewport.Size = config.sizePixels; // Set the size of the viewport
		//viewport.Usage = Viewport.UsageEnum.Usage2d; // Set to 2D mode
		viewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Once;
		viewport.CanvasItemDefaultTextureFilter = Viewport.DefaultCanvasItemTextureFilter.Nearest;	
		
		// Add the viewport to the scene tree (required for rendering)
		AddChild(viewport);

		foreach (var sprite in config.sprites)
		{
			try
			{
				sprite.TextureFilter = TextureFilterEnum.Nearest;
				viewport.AddChild(sprite);

			}
			catch (Exception e)
			{
				GD.Print("error3");
			}

		}

		// Wait until the frame has finished before getting the texture.
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);

		// Step 3: Extract the rendered image
		var tex = viewport.GetTexture();
		var image = tex.GetImage();

		outputTexture = ImageTexture.CreateFromImage(image);

		// Step 4: Save the image as a PNG
		Error err = image.SavePng(config.savePath);

		// Step 5: Clean up resources
		viewport.QueueFree();
		GD.Print("creating wfc");
	}

	public void run()
	{
		CreatePNG();
	}

}
