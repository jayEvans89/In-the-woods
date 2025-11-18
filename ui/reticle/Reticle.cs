using Godot;
using System;

namespace Ui
{
	public partial class Reticle : CenterContainer
	{
		[Export]
		public float DotRadius = 1.0f;

		[Export]
		public Color DotColor = Colors.White;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
    {
      QueueRedraw();
    }

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

    public override void _Draw()
    {
      DrawCircle(Vector2.Zero, DotRadius, DotColor);
    }
	}
}