using Godot;
using State;
using System;

namespace State
{
	[GlobalClass, Icon("res://state/IdlePlayerState/IdlePlayerState.svg")]
	public partial class IdlePlayerState : PlayerMovementState
	{
		public override void Enter()
		{
			Animation.Pause();
			Globals.Debug.AddProperty("Movement Speed", "0", 1);
		}

		public override void Update(float delta)
		{
			Player.UpdateGravity(delta);
			Player.UpdateInput(1, 1, 1);
			Player.UpdateVelocity();

			if (Input.IsActionJustPressed("crouch") && Player.IsOnFloor())
			{
				EmitSignal(SignalName.Transition, "CrouchingPlayerState");
			}

			if (Player.Velocity.Length() > 0.0f && Player.IsOnFloor())
			{
				EmitSignal(SignalName.Transition, "WalkingPlayerState");
			}

			if (Input.IsActionJustPressed("jump") && Player.IsOnFloor())
			{
				EmitSignal(SignalName.Transition, "JumpingPlayerState");
			}
		}
	}
}