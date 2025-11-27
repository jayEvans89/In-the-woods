using Godot;
using System;

namespace State
{
	[GlobalClass, Icon("res://state/SprintingPlayerState/SprintingPlayerState.svg")]
	public partial class SprintingPlayerState : PlayerMovementState
	{
		[Export]
		public float Speed = 7.0f;
		[Export]
		public float Acceleration = 0.1f;
		[Export]
		public float Deceleration = 0.25f;
		[Export]
		public float TopAnimationSpeed = 1.6f
;
		public override void Enter()
		{
			Animation.Play("Sprinting", -1.0, 1.0f);
			Globals.Debug.AddProperty("Movement Speed", Speed.ToString(), 1);
		}

		public override void Exit()
		{
			Animation.SpeedScale = 1.0f;
		}

		public override void Update(float delta)
		{
			Player.UpdateGravity(delta);
			Player.UpdateInput(Speed, Acceleration, Deceleration);
			Player.UpdateVelocity();

			SetAnimationSpeed(Player.Velocity.Length());

			if (Input.IsActionJustReleased("sprint"))
			{
				EmitSignal(SignalName.Transition, "WalkingPlayerState");
			}

			if (Input.IsActionJustPressed("jump") && Player.IsOnFloor())
			{
				EmitSignal(SignalName.Transition, "JumpingPlayerState");
			}
		}

		public void SetAnimationSpeed(float speed)
		{
			var alpha = Mathf.Remap(speed, 0.0, Speed, 0.0f, 1.0f);
			Animation.SpeedScale = Mathf.Lerp(0.0f, TopAnimationSpeed, (float)alpha);
		}
	}
}