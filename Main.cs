using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class Main : Control
{
	// Called when the node enters the scene tree for the first time.
	public static Main instance { get; private set; }

    public void RefreshDisplay()
    {
        Environment.RefreshDisplay();
        UpdateCode();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
    public override void _Ready()
    {
        instance = this;
    }

    public void OpenDialogue()
    {
        var filed = GetNode<FileDialog>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/FileDialog");
        if (GetNode<LineEdit>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/LineEdit").Text != "")
            filed.Popup();
        else if (GetNode<LineEdit>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/LineEdit").Text == "")
        {
            GetNode<AcceptDialog>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/AcceptDialog").Popup();
            return;
        }
    }

    public void SaveFile(string path)
    {
        var filename = GetNode<LineEdit>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/LineEdit").Text;
        using (StreamWriter sw = File.CreateText(Path.Combine(path, $"{filename}.sk")))
        {
            foreach (var line in GetCode())
            {
                sw.WriteLine(line);
            }
        }
    }
    public void ParticleScripts()
    {
        var filed = GetNode<FileDialog>("FileDialog");
        filed.Popup();
    }
    public void ParticleScriptFile(string path)
    {
        var filename = "ParticleScripts";
        using (StreamWriter sw = File.CreateText(Path.Combine(path, $"{filename}.sk")))
        {
            var file = Godot.FileAccess.Open("res://ParticleScripts.txt", Godot.FileAccess.ModeFlags.Read);
            while (file.GetPosition() < file.GetLength())
            {
                sw.WriteLine(file.GetLine());
            }
        }
    }
    public void UpdateCode()
    {
        var codebox = GetNode<CodeEdit>("Panel/HBoxContainer/VBoxContainer/TabContainer/Code");
        var scriptsT = new List<string>();
        var name = GetNode<LineEdit>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/LineEdit").Text;

        scriptsT.Add("function " + name + "(l: location):");
        if (Environment.instance.GetNode<Camera3D>("Camera3D2").Current)
            scriptsT.Add("    set pitch of {_l} to -90");
        foreach (var child in Subfunctions.instance.GetChildren())
        {
            var s = child.ReturnScript(false);
            foreach (var i in s)
                scriptsT.Add(i);
        }
        var strin = "";
        foreach (var line in scriptsT)
        {
            strin += $"\n{line}";
        }
        codebox.Text = strin;
    }
    List<string> GetCode()
    {
        var scriptsT = new List<string>();
        var name = GetNode<LineEdit>("Panel/HBoxContainer/Information/VBoxContainer/HBoxContainer/LineEdit").Text;

        scriptsT.Add("function " + name + "(l: location):");
        if (Environment.instance.GetNode<Camera3D>("Camera3D2").Current)
            scriptsT.Add("    set pitch of {_l} to -90");
        foreach (var child in Subfunctions.instance.GetChildren())
        {
            var s = child.ReturnScript(false);
            foreach (var i in s)
                scriptsT.Add(i);
        }
        return scriptsT;
    }

}
