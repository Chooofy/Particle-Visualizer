using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Hitbubble : Control, ISubf
{
    // Called when the node enters the scene tree for the first time.
    public void LoadOptions()
    {

    }
    public void PrintParticles(ref Node3D obj)
    {
        Node p = ResourceLoader.Load<PackedScene>("PSchem2.tscn").Instantiate();
        obj.AddChild(p);
        p.GetChild<CsgSphere3D>(0).Radius = 0.15f * ReturnLine()[1].ToFloat();
    }
    public List<string> ReturnLine()
    {
        var damage = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var radius = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;

        return new List<string>
        {
            damage,
            radius
        };
    }
    public List<string> ReturnScript(bool child = false)
    {
        var script = new List<string>();

        if (!child)
        {
            var k = "    Hitbubble({_l}," + $" {ReturnLine()[0]}, {ReturnLine()[1]}," + " {_l::*})";
            script.Add(k);
            return script;
        }
        else
        {
            var a = $"({ReturnLine()[0]}, {ReturnLine()[1]})";
            var b = "Hitbubble";
            script.Add(a);
            script.Add(b);
            return script;
        }
    }
}
