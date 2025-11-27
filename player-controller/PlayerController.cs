using Godot;
using System;

namespace Player
{
	public partial class PlayerController : CharacterBody3D
	{

		[Export]
		public float MouseSensitivity = 0.5f;
		[Export]
		public float TiltLowerLimit = Mathf.DegToRad(-90);
		[Export]
		public float TiltUpperLimit = Mathf.DegToRad(90);
		[Export]
		public Camera3D CameraController;
		[Export]
		public AnimationPlayer AnimationPlayer;
		[Export]
		public bool ToggleCrouch = true;

		public float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

		public bool MouseInput = false;
		public Vector3 MouseRotation;
		public float RotationInput;
		public float TiltInput;
		public Vector3 PlayerRotation;
		public Vector3 CameraRotation;

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("exit"))
			{
				GetTree().Quit();
			}
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			MouseInput = @event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured;

			if (!MouseInput)
			{
				return;
			}

			RotationInput = (@event as InputEventMouseMotion).Relative.X * MouseSensitivity;
			TiltInput = (@event as InputEventMouseMotion).Relative.Y * MouseSensitivity;
			GD.Print(new Vector2(RotationInput, TiltInput));
		}

		public override void _Ready()
		{
			// Set the mouse mode to captured.
			Input.MouseMode = Input.MouseModeEnum.Captured;
			AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		}

		public override void _PhysicsProcess(double delta)
		{
			Globals.Debug.AddProperty("Velocity", Velocity.ToString("2f"), 2);

			if (Input.IsActionJustPressed("toggle_mouse_mode"))
			{
				if (Input.MouseMode == Input.MouseModeEnum.Captured)
				{
					Input.MouseMode = Input.MouseModeEnum.Visible;
				}
				else
				{
					Input.MouseMode = Input.MouseModeEnum.Captured;
				}
			}

			UpdateCamera((float)delta);
		}

		public void UpdateCamera(float delta)
		{
			MouseRotation.X -= TiltInput * delta;
			MouseRotation.X = Mathf.Clamp(MouseRotation.X, TiltLowerLimit, TiltUpperLimit);
			MouseRotation.Y -= RotationInput * delta;

			PlayerRotation = new Vector3(0, MouseRotation.Y, 0);
			CameraRotation = new Vector3(MouseRotation.X, 0, 0);

			Transform3D transform = CameraController.Transform;
			transform.Basis = Basis.FromEuler(CameraRotation);
			CameraController.Transform = transform;

			Vector3 UpdatedCameraRotation = CameraController.Rotation;
			UpdatedCameraRotation.Z = 0;
			CameraController.Rotation = UpdatedCameraRotation;

			Transform3D playerTransform = GlobalTransform;
			playerTransform.Basis = Basis.FromEuler(PlayerRotation);
			GlobalTransform = playerTransform;

			RotationInput = 0;
			TiltInput = 0;
		}

		public void UpdateGravity(double delta)
		{
			Vector3 velocity = Velocity;
			velocity.Y -= Gravity * (float)delta;
			Velocity = velocity;
		}

		public void UpdateInput(float speed, float acceleration, float deceleration)
		{
			Vector3 velocity = Velocity;

			Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
			Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Vector3.Zero)
			{
				velocity.X = Mathf.Lerp(velocity.X, direction.X * speed, acceleration);
				velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * speed, acceleration);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, deceleration);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, deceleration);
			}

			Velocity = velocity;
		}

		public void UpdateVelocity()
		{
			MoveAndSlide();
		}
	}
}