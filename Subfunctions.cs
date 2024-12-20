using Godot;
using System;
using System.Collections.Generic;

public partial class Subfunctions : Control
{
    // Called when the node enters the scene tree for the first time.

    OptionButton list;
    public VBoxContainer fList;
    public static Subfunctions instance;

    void SetNodes()
    {
        list = GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton");
        fList = GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/FSubList");
    }
    public override void _Ready()
    {
        instance = this;
        SetNodes();

        list.Clear();
        foreach (var item in SF)
        {
            list.AddItem(item);
            list.AddSeparator();
        }
    }
    void AddFunction(int n)
    {
        var item = list.GetItemText(n);
        var fn = ResourceLoader.Load<PackedScene>($"Subfunctions/{item}.tscn").Instantiate();
        fList.AddChild(fn);
        LoadOptions(fn as ISubf); 
        fn.GetNode<LineEdit>("HBoxContainer2/Delay2").Editable = true;
    }
    public List<ISubf> GetChildren()
    {
        List<ISubf> ret = new List<ISubf>();
        foreach (ISubf x in GetNode<VBoxContainer>("Panel/VBoxContainer/ScrollContainer/FSubList").GetChildren())
        {
            ret.Add(x);
        }
        return ret;
    }
    public static void LoadOptions(ISubf fn)
    {
        fn.LoadOptions();
    }
    public static List<string> SF = new List<string>
    {
        "Bolt",
        "Line",
        "SineWave",
        "Polygon",
        "Circle",
        "Log",
        "Helix",
        "Hitbubble",
        "CircleF",
        "CircleFS",
        "SineF",
        "LineF"
    };

}
