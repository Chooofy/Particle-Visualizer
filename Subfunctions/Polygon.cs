using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Polygon : Control, ISubf
{
    // Called when the node enters the scene tree for the first time.

    public void LoadOptions()
    {
        var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
        foreach (var j in ParticleOptions.GetParticleOptions())
        {
            op.AddIconItem(ParticleOptions.GetImage(j), Path.GetFileNameWithoutExtension(j));
        }
    }
    public void PrintParticles(ref Node3D obj)
    {
        var Info = ReturnLine();
        var points = Info[2].ToFloat();
        var r = Info[1].ToFloat();
        Vector2 rotation = new Vector2(1, 0);
        rotation = rotation.Rotated((float)(Info[3].ToFloat() * Math.PI / 180));
        for (double i = 0; i <= points; i += 1)
        {

            var initial = rotation;
            rotation = rotation.Rotated((float)(2*Math.PI/points));

            Line(Info, ref obj, initial, rotation);
            //Stuff
        }
    }

    public void Line(List<string> Info, ref Node3D obj, Vector2 initial, Vector2 final)
    {

        var slope = (final.Y - initial.Y)/(final.X - initial.X);
        var density = 4*Info[1].ToFloat();

        for (int i = 0; i < density; i++)
        {
            var dx = (float)i * (final.X - initial.X)/density;
            var dy = slope * dx;
            Node p = ResourceLoader.Load<PackedScene>("ParticleSchem.tscn").Instantiate();
            obj.AddChild(p);
            //Particle Type Here
            var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
            p.GetNode<Sprite3D>("Sprite3D").Texture = op.GetItemIcon(op.Selected);
            if (Info[0] == "dust" || Info[0] == "dust_color_transition" || Info[0] == "trail") { p.GetNode<Sprite3D>("Sprite3D").Modulate = GetNode<ColorPickerButton>("HBoxContainer2/ColorPickerButton").Color; }
            p.Set(Node3D.PropertyName.Position, new Vector3(Info[1].ToFloat() * (initial.X + dx), Info[1].ToFloat() * (initial.Y + dy), 0));
        }
            
    }
    public List<string> ReturnLine()
    {
        var particle = GetNode<OptionButton>("HBoxContainer/OptionButton").Text;
        var radius = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;
        var points = GetNode<LineEdit>("HBoxContainer/LineEdit3").Text;
        var ip = GetNode<LineEdit>("HBoxContainer/LineEdit4").Text;

        return new List<string>
        {
            particle,
            radius,
            points,
            ip
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Polygon({_l}," + $" \"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[2]}, {ReturnLine()[3]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"(\"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[2]}, {ReturnLine()[3]}, {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            var b = "Polygon";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}
