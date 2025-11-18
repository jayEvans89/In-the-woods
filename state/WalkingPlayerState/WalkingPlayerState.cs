using Godot;
using State;
using System;

namespace State
{
	public partial class WalkingPlayerState : PlayerMovementState
	{
		[Export]
		public float Speed = 5.0f;
		[Export]
		public float Acceleration = 0.1f;
		[Export]
		public float Deceleration = 0.25f;
		[Export]
		public float TopAnimationSpeed = 2.2f;
		public override void Enter()
		{
			Animation.Play("Walking", -1.0, 1.0f);
			Globals.Debug.AddProperty("Movement Speed", Speed.ToString(), 1);
		}

		public override void Update(float delta)
		{
			Player.UpdateGravity(delta);
			Player.UpdateInput(Speed, Acceleration, Deceleration);
			Player.UpdateVelocity();

			SetAnimationSpeed(Player.Velocity.Length());

			if (@Input.IsActionPressed("sprint") && Player.IsOnFloor())
			{
				EmitSignal(SignalName.Transition, "SprintingPlayerState");
			}

			if (Input.IsActionJustPressed("jump") && Player.IsOnFloor())
			{
				EmitSignal(SignalName.Transition, "JumpingPlayerState");
			}

			if (Player.Velocity.Length() == 0.0f)
			{
				EmitSignal(SignalName.Transition, "IdlePlayerState");
			}
		}

		public void SetAnimationSpeed(float speed)
		{
			var alpha = Mathf.Remap(speed, 0.0, Speed, 0.0f, 1.0f);
			Animation.SpeedScale = Mathf.Lerp(0.0f, TopAnimationSpeed, (float)alpha);
		}
	}
}