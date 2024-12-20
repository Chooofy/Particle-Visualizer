using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Circle : Control, ISubf
{
    // Called when the node enters the scene tree for the first time.
    bool edit = false;
    public void LoadOptions()
    {
        var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
        foreach (var j in ParticleOptions.GetParticleOptions())
        {
            op.AddIconItem(ParticleOptions.GetImage(j), Path.GetFileNameWithoutExtension(j));
        }
    }
    void EditDensity()
    {
        edit = true;
    }
    void ExitDensity()
    {
        if (GetNode<LineEdit>("HBoxContainer/LineEdit3").Text == "0.") { GetNode<LineEdit>("HBoxContainer/LineEdit3").Text = "0.5"; }
        edit = false;
    }
    public void PrintParticles(ref Node3D obj)
    {
        if (edit == false)
        {
            var Info = ReturnLine();
            Vector2 rotation = new Vector2(0, 1);
            float density = Info[2].ToFloat();
            for (float i = 0; i < 36; i += density)
            {
                Node p = ResourceLoader.Load<PackedScene>("ParticleSchem.tscn").Instantiate();
                obj.AddChild(p);
                //Particle Type Here
                var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
                p.GetNode<Sprite3D>("Sprite3D").Texture = op.GetItemIcon(op.Selected);
                rotation = rotation.Rotated((float)Math.PI / (9/density));
                if (Info[0] == "dust" || Info[0] == "dust_color_transition" || Info[0] == "trail") { p.GetNode<Sprite3D>("Sprite3D").Modulate = GetNode<ColorPickerButton>("HBoxContainer2/ColorPickerButton").Color; }

                p.Set(Node3D.PropertyName.Position, new Vector3(Info[1].ToFloat() * rotation.X, Info[1].ToFloat() * rotation.Y, 0));
            }
        }
    }
    public List<string> ReturnLine()
    {
        var particle = GetNode<OptionButton>("HBoxContainer/OptionButton").Text;
        var radius = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;
        var density = GetNode<LineEdit>("HBoxContainer/LineEdit3").Text;
        return new List<string>
        {
            particle,
            radius,
            density
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Circle({_l}," + $" \"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[2]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"(\"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[2]}, {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            var b = "Circle";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}
