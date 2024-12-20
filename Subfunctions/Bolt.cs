using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Bolt : Control, ISubf
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
    bool edit = false;
    void EditDensity()
    {
        edit = true;
    }
    void ExitDensity()
    {
        edit = false;
    }
    public void PrintParticles(ref Node3D obj)
    {
        if (!edit)
        {
            var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text.ToFloat();
            var vertices = GetNode<LineEdit>("HBoxContainer2/LineEdit").Text.ToFloat();
            var density = GetNode<LineEdit>("HBoxContainer2/LineEdit2").Text.ToFloat();
            var amplitude = GetNode<LineEdit>("HBoxContainer2/LineEdit3").Text.ToFloat();
            List<Vector3> points = new List<Vector3>();
            points.Add(new Vector3(0, 0, 0));
            var dXX = distance / (vertices);
            for (int i = 0; i < vertices; i++)
            {
                var dx = (float)(amplitude * GD.RandRange(-1d, 1d));
                var dy = (float)(amplitude * GD.RandRange(-1d, 1d));
                var z = (i+1) * distance / vertices;
                GD.Print(z);
                points.Add(new Vector3(dx, dy, (float)-z));
            }
            points.Add(new Vector3(0, 0, -distance));
            for (int i = 0; i < points.Count - 1; i++)
            {
                Line(ReturnLine(), ref obj, points[i], points[i + 1]);
            }
        }
    }
    public void Line(List<string> Info, ref Node3D obj, Vector3 initial, Vector3 final)
    {

        var slope = (final.Y - initial.Y) / (final.X - initial.X);
        var density = 5/Info[2].ToFloat();

        for (int i = 0; i < density; i++)
        {
            var dx = (float)i * (final.X - initial.X) / density;
            var dy = slope * dx;
            var dz = (float)i * (final.Z - initial.Z) / density;
            Node p = ResourceLoader.Load<PackedScene>("ParticleSchem.tscn").Instantiate();
            obj.AddChild(p);
            //Particle Type Here
            var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
            p.GetNode<Sprite3D>("Sprite3D").Texture = op.GetItemIcon(op.Selected);
            if (Info[0] == "dust" || Info[0] == "dust_color_transition" || Info[0] == "trail") { p.GetNode<Sprite3D>("Sprite3D").Modulate = GetNode<ColorPickerButton>("HBoxContainer2/ColorPickerButton").Color; }

            p.Set(Node3D.PropertyName.Position, new Vector3((initial.X + dx), (initial.Y + dy), (initial.Z + dz)));
        }

    }
    public List<string> ReturnLine()
    {
        var particle = GetNode<OptionButton>("HBoxContainer/OptionButton").Text;
        var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var vertices = GetNode<LineEdit>("HBoxContainer2/LineEdit").Text;
        var density = GetNode<LineEdit>("HBoxContainer2/LineEdit2").Text;
        var amplitude = GetNode<LineEdit>("HBoxContainer2/LineEdit3").Text;
        return new List<string>
        {
            particle,
            distance,
            density,
            amplitude,
            vertices
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Bolt({_l}," + $" \"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[4]}, {ReturnLine()[2]}, {ReturnLine()[3]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"(\"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[4]}, {ReturnLine()[2]}, {ReturnLine()[3]}, {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            var b = "Bolt";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}
