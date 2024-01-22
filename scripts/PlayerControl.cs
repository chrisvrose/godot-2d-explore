using System;
using Godot;
enum PlayerState
{
	NOCHANGE,
	IDLE,
	WALK,
	JUMP
}

public partial class PlayerControl : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -200.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		PlayerState playerState = PlayerState.NOCHANGE;
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			playerState = PlayerState.JUMP;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		// PLEASSE NOTE WE IGNORE THE Y DIRECTION HERE
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_jump", "ui_down");
		if (direction != Vector2.Zero)
		{
			if (playerState != PlayerState.JUMP) playerState = PlayerState.WALK;
			velocity.X = direction.X * Speed;
		}
		else
		{
			if (playerState != PlayerState.JUMP) playerState = PlayerState.IDLE;
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		if(playerState!=PlayerState.NOCHANGE) CallDeferred(MethodName.setAnimationState,Variant.From(playerState));
		CallDeferred(MethodName.setAnimationDirection, direction); 

		Velocity = velocity;
		MoveAndSlide();
	}
	private void setAnimationState(PlayerState playerState)
	{
		switch (playerState)
		{
			case PlayerState.WALK:
				{
					animatedSprite2D.Animation = "walk";
					break;
				}
			case PlayerState.JUMP:
				{
					animatedSprite2D.Animation = "jump";
					break;
				}
			case PlayerState.IDLE:
				{
					animatedSprite2D.Animation = "idle";
					break;
				}
			default:break;
		}
	}

	private void setAnimationDirection(Vector2 inputDirection)
	{
		if (inputDirection.X < 0)
		{
			animatedSprite2D.FlipH = true;
			return;
		}
		if (inputDirection.X > 0)
		{
			animatedSprite2D.FlipH = false;
		}
	}

}
