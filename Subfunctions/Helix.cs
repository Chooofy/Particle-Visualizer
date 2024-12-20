using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public partial class Helix : Control, ISubf
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
    public string type;
    public string direction;
    public bool edit;
    void EditDensity()
    {
        edit = true;
    }
    void ExitDensity()
    {
        edit = false;
    }
    public void SwitchDirection()
    {
        if (GetNode<CheckButton>("HBoxContainer2/CheckButton").ButtonPressed == true) { direction = "Horizontal"; }
        else { direction = "Vertical"; }
    }
    public void SwitchType(int n)
    {
        var box = GetNode<OptionButton>("HBoxContainer2/OptionButton").GetItemText(n);
        if (box == "Sine")
        {
            type = "Sine";
        }
        else
        {
            type = "Cosine";
        }
    }

    public void PrintParticles(ref Node3D obj)
    {
        if (edit == false)
        {
            var Info = ReturnLine();
            var x = (double)Info[4].ToFloat();
            for (double i = 0; i < Info[1].ToFloat(); i += x)
            {
                Node p = ResourceLoader.Load<PackedScene>("ParticleSchem.tscn").Instantiate();
                obj.AddChild(p);
                var y = (type == "Sine") ? Info[2].ToFloat() * Math.Sin(2 * Math.PI / Info[3].ToFloat() * (i / 9)) : Info[2].ToFloat() * Math.Cos(2 * Math.PI / Info[3].ToFloat() * (i / 9));
                var x2 = (type == "Sine") ? Info[2].ToFloat() * Math.Cos(2 * Math.PI / Info[3].ToFloat() * (i / 9)) : Info[2].ToFloat() * Math.Sin(2 * Math.PI / Info[3].ToFloat() * (i / 9));

                //Particle Type Here
                var op = GetNode<OptionButton>("HBoxContainer/OptionButton");
                p.GetNode<Sprite3D>("Sprite3D").Texture = op.GetItemIcon(op.Selected);
                if (Info[0] == "dust" || Info[0] == "dust_color_transition" || Info[0] == "trail") { p.GetNode<Sprite3D>("Sprite3D").Modulate = GetNode<ColorPickerButton>("HBoxContainer2/ColorPickerButton").Color; }

                if (direction != "Horizontal")
                    p.Set(Node3D.PropertyName.Position, new Vector3((float)x2, (float)y, (float)-i));
                else
                    p.Set(Node3D.PropertyName.Position, new Vector3((float)y, (float)x2, (float)-i));

            }
        }
    }
    public List<string> ReturnLine()
    {
        var particle = GetNode<OptionButton>("HBoxContainer/OptionButton").Text;
        var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var period = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;
        var amplitude = GetNode<LineEdit>("HBoxContainer/LineEdit3").Text;
        var density = GetNode<LineEdit>("HBoxContainer2/LineEdit3").Text;
        return new List<string>
        {
            particle,
            distance,
            amplitude,
            period,
            density
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Helix({_l}," + $" \"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[3]}, {ReturnLine()[2]}, {ReturnLine()[4]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"(\"{ReturnLine()[0]}\" parsed as a particle, {ReturnLine()[1]}, {ReturnLine()[3]}, {ReturnLine()[2]}, {ReturnLine()[4]}, {GetNode<LineEdit>("HBoxContainer2/Delay2").Text}, {GetNode<LineEdit>("HBoxContainer2/FN2").Text})";
            var b = "Helix";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}
