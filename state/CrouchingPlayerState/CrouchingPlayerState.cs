using Godot;
using System;
using System.Threading.Tasks;

namespace State
{
	[GlobalClass, Icon("res://state/CrouchingPlayerState/CrouchingPlayerState.svg")]
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
		[Export]
		public ShapeCast3D CrouchShapeCast;

		public bool isCrouching = true;

		public override void Enter()
		{
			Animation.Play("Crouching", -1.0, CrouchAnimationSpeed);
			Globals.Debug.AddProperty("Movement Speed", Speed.ToString(), 1);

			if (Player.ToggleCrouch)
			{
				isCrouching = true;
			}
		}

		public override async void Update(float delta)
		{
			Player.UpdateGravity(delta);
			Player.UpdateInput(Speed, Acceleration, Deceleration);
			Player.UpdateVelocity();

			if (Input.IsActionJustReleased("crouch") && Player.IsOnFloor())
			{
				if (!Player.ToggleCrouch)
				{
					await UnCrouch();
					return;
				}
			}

			if (Input.IsActionJustPressed("crouch") && Player.IsOnFloor())
			{
				await UnCrouch();
			}
		}

		private async Task UnCrouch()
		{
			if (!CrouchShapeCast.IsColliding())
			{
				Animation.Play("Crouching", -1.0, -CrouchAnimationSpeed * 1.4f, true);

				if (Animation.IsPlaying())
				{
					await ToSignal(Animation, AnimationMixer.SignalName.AnimationFinished);
				}

				EmitSignal(SignalName.Transition, "IdlePlayerState");
			}
			else if (CrouchShapeCast.IsColliding())
			{
				await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
				await UnCrouch();
			}
		}
	}
}