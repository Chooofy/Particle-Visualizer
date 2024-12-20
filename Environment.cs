using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Environment : Node3D
{
	// Called when the node enters the scene tree for the first time.

	public static Environment instance;
	public override void _Ready()
	{
		instance = this;
		SetCameras();
		axis = GetNode<Node3D>("Axis");
    }
    public void SwitchCamera(int n)
    {
		SetCameras();
		GetNode<Node3D>("Axis/Particles").Rotation = new Vector3(0, 0, 0);
        if (n == 0)
		{
			Cameras[0].MakeCurrent();
			Cameras[1].Current = false;
			Cameras[2].Current = false;
		}
        if (n == 1)
        {
            Cameras[1].MakeCurrent();
            GetNode<Node3D>("Axis/Particles").Rotation = new Vector3((float)Math.PI/2, 0, 0);
            Cameras[0].Current = false;
            Cameras[2].Current = false;
        }
        if (n == 2)
        {
            Cameras[2].MakeCurrent();
            Cameras[1].Current = false;
            Cameras[0].Current = false;
        }

    }

	List<Camera3D> Cameras = new List<Camera3D>();


    void SetCameras()
	{
		Cameras.Clear();
		Cameras.Add(GetNode<Camera3D>("Camera3D"));
		Cameras.Add(GetNode<Camera3D>("Camera3D2"));
		Cameras.Add(GetNode<Camera3D>("Camera3D3"));
	}

	public static Node3D axis;
	public static int delta = 0;
    public static void RefreshDisplay()
	{
		delta++;
		var particles = Environment.axis.GetNode<Node3D>("Particles");
		foreach (var child in particles.GetChildren())
		{
			child.QueueFree();
		}

		foreach (ISubf child in Subfunctions.instance.fList.GetChildren())
		{
			child.PrintParticles(ref particles);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
