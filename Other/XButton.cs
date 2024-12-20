using Godot;
using System;

public partial class XButton : Control
{
	// Called when the node enters the scene tree for the first time.
	
	void Pressed()
	{
		GetParent().GetParent().QueueFree();
	}
}
