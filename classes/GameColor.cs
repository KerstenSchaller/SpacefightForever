using Godot;
using System;

public static class GameColor
{
    static bool initialized = false;
    static Color color0;
    static Color color1;
    static Color color2;
    static Color color3;
    static Color color4;

    public static Color Color0
    {
        get
        {
            if(initialized == false)init();
            return color0;
        }
    }

    public static Color Color1
    {
        get
        {
            if(initialized == false)init();
            return color1;
        }
    }

    public static Color Color2
    {
        get
        {
            if(initialized == false)init();
            return color2;
        }
    }

    public static Color Color3
    {
        get
        {
            if(initialized == false)init();
            return color3;
        }
    }

    public static Color Color4
    {
        get
        {
            if(initialized == false)init();
            return color4;
        }
    }

    static void init()
    {
        /*
        Image image = Image.LoadFromFile("res://palettes/ink-1x.png");
        color3 =  image.GetPixel(4,0);
        GD.Print(color3);
        */

        color0 = new Color (0.12156863f, 0.12156863f, 0.16078432f, 1);
        color1 = new Color (0.25490198f, 0.22745098f, 0.25882354f, 1);
        color2 = new Color (0.34901962f, 0.3764706f, 0.4392157f, 1);
        color3 = new Color (0.5882353f, 0.63529414f, 0.7019608f, 1);
        color4 = new Color (0.91764706f, 0.9411765f, 0.84705883f, 1);
        
        initialized = true;
    }
}