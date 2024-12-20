using Godot;
using System;
using System.Collections.Generic;

public partial class SineF : Control, ISubf
{
    // Called when the node enters the scene tree for the first time.
    public string type;
    public string direction;
    public bool edit;
    public bool d2;
    void EditDensity()
    {
        edit = true;
    }
    void ExitDensity()
    {
        edit = false;
    }
    public void LoadOptions()
    {
        direction = "Vertical";
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
    public void SwitchDirection()
    {
        if (GetNode<CheckButton>("HBoxContainer2/CheckButton").ButtonPressed == true) { direction = "Horizontal"; d2 = false; }
        else { direction = "Vertical"; d2 = true; }
    }
    public void SwitchType(int n)
    {
        var box = GetNode<OptionButton>("HBoxContainer2/OptionButton").Text;
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
            float density = ReturnLine()[3].ToFloat();
            float distance = ReturnLine()[0].ToFloat();

   
            for (float i = 0; i < distance; i += density)
            {
                Node3D n = new Node3D();
                obj.AddChild(n);
                n.Position = obj.Position;
                var y = (type == "Sine") ? Info[1].ToFloat() * Math.Sin(2 * Math.PI / Info[2].ToFloat() * (i / 9)) : Info[1].ToFloat() * Math.Cos(2 * Math.PI / Info[2].ToFloat() * (i / 9));
                n.Position = (direction == "Vertical") ? new Vector3(0, (float)y, -i) : new Vector3((float)y, 0, -i);
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
        var distance = GetNode<LineEdit>("HBoxContainer/LineEdit").Text;
        var period = GetNode<LineEdit>("HBoxContainer2/LineEdit2").Text;
        var amplitude = GetNode<LineEdit>("HBoxContainer2/LineEdit3").Text;
        var density = GetNode<LineEdit>("HBoxContainer/LineEdit2").Text;
        return new List<string>
        {
            distance,
            amplitude,
            period,
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
        script.Add("        SineF({_l}, loop-value, {_parameters::%{_n}%::*}, " + $"\"{type}\", {d2.ToString().ToLower()}, {ReturnLine()[0]}, {ReturnLine()[1]}, {ReturnLine()[2]}, {ReturnLine()[3]}, vector(0,0,0), {GetNode<LineEdit>("HBoxContainer2/Delay2").Text})");
        return script;
    }
}
