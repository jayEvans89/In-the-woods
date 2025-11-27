using Godot;
using System;

namespace State
{
	[GlobalClass, Icon("res://state/JumpingPlayerState/JumpingPlayerState.svg")]
	public partial class JumpingPlayerState : PlayerMovementState
	{
		[Export]
		public float Speed = 6.0f;

		[Export]
		public float Acceleration = 0.1f;

		[Export]
		public float Deceleration = 0.25f;

		[Export]
		public float JumpVelocity = 4.5f;

		[Export(PropertyHint.Range, "0.5, 1.0, 0.01")]
		public float InputMultiplier = 0.85f;

		public override void Enter()
		{
			var velocity = Player.Velocity;
			velocity.Y = JumpVelocity;
			Player.Velocity = velocity;
			Animation.Play("JumpStart");
		}

    public override void Update(float delta)
    {
			Player.UpdateGravity(delta);
			Player.UpdateInput(Speed * InputMultiplier, Acceleration, Deceleration);
			Player.UpdateVelocity();

			if (Player.IsOnFloor())
      {
        EmitSignal(SignalName.Transition, "IdlePlayerState");
      }
    }
	}
}