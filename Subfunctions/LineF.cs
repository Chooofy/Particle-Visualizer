using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class LineF : Control, ISubf
{
	// Called when the node enters the scene tree for the first time.
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void LoadOptions()
    {
        foreach (var i in Subfunctions.SF)
            if (i != "LineF" && i != "CircleF" && i != "CircleFS" && i != "SineF")
                GetNode<OptionButton>("HBoxContainer/OptionButton").AddItem(i);
    }

    public void SelectItem(int n)
    {
        Node node = GetNode<VBoxContainer>("ScrollContainer/Composite");

        Node p = ResourceLoader.Load<PackedScene>($"Subfunctions/{GetNode<OptionButton>("HBoxContainer/OptionButton").Text}.tscn").Instantiate();
        node.AddChild(p);
        //p.GetNode<LineEdit>("HBoxContainer2/Delay2").Editable = false;
        Subfunctions.LoadOptions((ISubf)p);
    }
    public void PrintParticles(ref Node3D obj)
    {
        float density = ReturnLine()[1].ToFloat();
        float distance = ReturnLine()[0].ToFloat();
 
        for (float i = 0; i < distance; i+=density)
        {
            Node3D n = new Node3D();
            obj.AddChild(n);
            n.Position = obj.Position;
            n.Position = new Vector3(0, 0, -i);
            foreach (ISubf subf in GetChildren())
            {
                subf.PrintParticles(ref n);
            }
        }
    }
    public List<ISubf> GetChildren()
    {
        List<ISubf> ret = new List<ISubf>();
        foreach (ISubf x in GetNode<VBoxContainer>("ScrollContainer/Composite").GetChildren())
        {
            ret.Add(x);
        }
        return ret;
    }
    public List<string> ReturnLine()
    {
        var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var density = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;

        return new List<string>
        {
            distance,
            density
        };
    }
    public List<string> ReturnScript(bool childe = false)
    {
        var script = new List<string>();
        List<string> parameters = new List<string>();
        List<string> composites = new List<string>();

        foreach (var child in GetChildren())
        {
            var list = child.ReturnScript(true);
            composites.Add(list[1]);
            parameters.Add(list[0]);
        }
        var co = "(";
        var n = 0;
        foreach (var c in composites)
        {
            if (n != composites.Count - 1)
                co += $"\"{c}\"" + ", ";
            else
                co += $"\"{c}\"";
            n++;
        }
        co += ")";

        script.Add("    set {_cf::*} to " + co);
        n = 0;
        foreach (var para in parameters)
        {
            n++;
            var txt = "    set {_parameters::" + $"{n}::*" + "} to " + $"{para}";
            script.Add(txt);
        }
        script.Add("    set {_n} to 0\r\n    loop {_cf::*}:\r\n        add 1 to {_n}");
        script.Add("        LineF({_l}, loop-value, {_parameters::%{_n}%::*}, " + $"{ReturnLine()[0]}, {ReturnLine()[1]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text})");
        return script;
    }
}
