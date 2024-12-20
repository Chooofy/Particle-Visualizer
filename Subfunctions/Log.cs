using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Log : Control, ISubf
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
    void EditDensity()
    {
        edit = true;
    }
    void ExitDensity()
    {
        edit = false;
    }
    bool edit = false;
    public void PrintParticles(ref Node3D obj)
    {
        var Info = ReturnLine();
        var density = 0.1d;
        try
        {
            density = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text.ToFloat();
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }
        if (!edit)
        {
            for (double i = 0; i < Info[1].ToFloat(); i += density)
            {
                Node p = ResourceLoader.Load<PackedScene>("ParticleSchem.tscn").Instantiate();
                obj.AddChild(p);
                var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
                p.GetNode<Sprite3D>("Sprite3D").Texture = op.GetItemIcon(op.Selected);
                if (Info[0] == "dust" || Info[0] == "dust_color_transition" || Info[0] == "trail") { p.GetNode<Sprite3D>("Sprite3D").Modulate = GetNode<ColorPickerButton>("HBoxContainer2/ColorPickerButton").Color; }

                //Particle Type Here

                var y = (Info[3] == "Logarithm") ? Info[2].ToFloat() * Math.Log(i) : Info[2].ToFloat() * Math.Exp(i);

                p.Set(Node3D.PropertyName.Position, new Vector3(0, (float)y, (float)-i));
            }
        }
    }
    public List<string> ReturnLine()
    {
        var particle = GetNode<OptionButton>("HBoxContainer/OptionButton").Text;
        var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var StF = GetNode<LineEdit>("HBoxContainer2/LineEdit2").Text;
        var type = GetNode<OptionButton>("HBoxContainer2/OptionButton").Text;
        return new List<string>
        {
            particle,
            distance,
            StF,
            type
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Log({_l}," + $" \"{ReturnLine()[0]}\" parsed as a particle, \"{ReturnLine()[3]}\", {ReturnLine()[1]}, {ReturnLine()[2]}, {GetNode<LineEdit>("HBoxContainer/LineEdit2").Text}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"(\"{ReturnLine()[0]}\" parsed as a particle, \"{ReturnLine()[3]}\", {ReturnLine()[1]}, {ReturnLine()[2]}, {GetNode<LineEdit>("HBoxContainer/LineEdit2").Text}, {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            var b = "Log";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}
