using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class ParticleOptions : Node
{
    // Called when the node enters the scene tree for the first time.

    public static List<string> GetParticleOptions()
    {
        List<string> lis = new List<string>();
        int n = 0;
        foreach (var i in DirAccess.GetFilesAt("res://Particles"))
        {
            if (n % 2 == 0)
            {
                var name = Path.GetFileNameWithoutExtension(i);
                GD.Print(name);
                lis.Add(i);
            }
            n++;
        }
        return lis;
    }

    public static Texture2D GetImage(string name)
    {
        var image = ResourceLoader.Load<CompressedTexture2D>($"res://Particles/{name}");
        Texture2D texture = image;
        return texture;
    }


}
