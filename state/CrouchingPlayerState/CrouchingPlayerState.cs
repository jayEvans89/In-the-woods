using Godot;
using System;

namespace State
{
	public partial class CrouchingPlayerState : PlayerMovementState
	{
		[Export]
		public float Speed = 3.0f;
		[Export]
		public float Acceleration = 0.1f;
		[Export]
		public float Deceleration = 0.25f;
		[Export(PropertyHint.Range, "1, 6.0, 0.1")]
		public float CrouchAnimationSpeed = 4.0f;

		public override void Enter()
		{
			Animation.Play("Crouching", -1.0, CrouchAnimationSpeed);
			Globals.Debug.AddProperty("Movement Speed", Speed.ToString(), 1);
		}

		public override void Update(float delta)
		{
			Player.UpdateGravity(delta);
			Player.UpdateInput(Speed, Acceleration, Deceleration);
			Player.UpdateVelocity();

			if (Input.IsActionJustReleased("crouch") && Player.IsOnFloor())
			{
				UnCrouch();
			}
		}

		private void UnCrouch()
		{ }
	}
}