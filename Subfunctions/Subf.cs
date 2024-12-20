using Godot;
using System;
using System.Collections.Generic;

public interface ISubf
{
   public void PrintParticles(ref Node3D obj);
   public List<string> ReturnLine();
   public void LoadOptions();
   public List<string> ReturnScript(bool child = false);
}
