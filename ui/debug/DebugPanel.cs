using Godot;
using System;

public partial class DebugPanel : PanelContainer
{
	public VBoxContainer PropertiesContainer;
	public string FramesPerSecond = "0";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Globals.Debug = this;

		Visible = false;
		PropertiesContainer = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("debug"))
		{
			Visible = !Visible;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Visible)
		{
			return;
		}

		FramesPerSecond = (1.0 / delta).ToString("F2");
		AddProperty("FPS", FramesPerSecond, 0);

	}

	public void AddProperty(string title, string value, int order)
	{
		var target = PropertiesContainer.FindChild(title, true, false) as Label;
		if (target == null)
		{
			target = new Label();
			PropertiesContainer.AddChild(target);
			target.Name = title;
			target.Text = title + ": " + value;
		}
		else if (Visible)
		{
			target.Text = title + ": " + value;
			PropertiesContainer.MoveChild(target, order);
		}
	}
}
