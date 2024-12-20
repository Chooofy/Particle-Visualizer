using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class CircleF : Control, ISubf
{
    // Called when the node enters the scene tree for the first time.
    bool edit = false;
    public void LoadOptions()
    {
        foreach (var i in Subfunctions.SF)
            if (i != "LineF" && i != "CircleF" && i != "CircleFS" && i != "SineF")
                GetNode<OptionButton>("HBoxContainer/OptionButton").AddItem(i);
    }

    public void EditDensity()
    {
        edit = true;
    }
    public void ExitDensity()
    {
        if (GetNode<LineEdit>("HBoxContainer/LineEdit2").Text == "0.") { GetNode<LineEdit>("HBoxContainer/LineEdit2").Text = "0.5"; }
        edit = false;
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
        if (edit == false)
        {
            Vector3 rotation2 = new Vector3(0,(float)-Math.PI/2,0);
            float density = ReturnLine()[1].ToFloat();
            float radius = ReturnLine()[0].ToFloat();
            float angle = ReturnLine()[2].ToFloat();
            float dt = (float)Math.PI/2 + angle*(float)Math.PI/180 + Environment.delta *(float)Math.PI/180;
            Vector2 rotation = new Vector2(0, radius);
            rotation = rotation.Rotated((float)Math.PI * Environment.delta / 180);
            for (float i = 0; i < 36; i += density)
            {
                rotation = rotation.Rotated((float)Math.PI / (9 / density));
                dt += (float)Math.PI / (9 / density);
                var vector = new Vector3(rotation.X,rotation.Y, 0);
                Node3D n = new Node3D();
                obj.AddChild(n);
                n.Position = vector;
                rotation2.X = dt;
                n.Rotation = rotation2;
                foreach (ISubf subf in GetChildren())
                {
                    subf.PrintParticles(ref n);
                }
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
        var radius = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var density = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;
        var angle = GetNode<LineEdit>("HBoxContainer2/LineEdit").Text;
        return new List<string>
        {
            radius,
            density,
            angle
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
            var txt = "    set {_parameters::" + $"{n}::*"+"} to " + $"{para}";
            script.Add(txt);
        }
        script.Add("    set {_n} to 0\r\n    loop {_cf::*}:\r\n        add 1 to {_n}");
        script.Add("        CircleF({_l}, loop-value, {_parameters::%{_n}%::*}, " + $"{ReturnLine()[0]}, {ReturnLine()[1]}, {ReturnLine()[2]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text})");
        return script;
    }
}
