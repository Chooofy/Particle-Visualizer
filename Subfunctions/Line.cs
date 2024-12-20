using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Line : Control, ISubf
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
        for (double i = 0; i < Info[1].ToFloat(); i += 0.25d)
        {
            Node p = ResourceLoader.Load<PackedScene>("ParticleSchem.tscn").Instantiate();
            obj.AddChild(p);
            var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
            p.GetNode<Sprite3D>("Sprite3D").Texture = op.GetItemIcon(op.Selected);
            if (Info[0] == "dust" || Info[0] == "dust_color_transition" || Info[0] == "trail") { p.GetNode<Sprite3D>("Sprite3D").Modulate = GetNode<ColorPickerButton>("HBoxContainer2/ColorPickerButton").Color; }

            //Particle Type Here
            p.Set(Node3D.PropertyName.Position, new Vector3(0, 0, (float)-i));
        }
    }
    public List<string> ReturnLine()
    {
        var particle = GetNode<OptionButton>("HBoxContainer/OptionButton").Text;
        var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;

        return new List<string>
        {
            particle,
            distance
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Line({_l}," + $" \"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, 1, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"(\"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            var b = "Line";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}