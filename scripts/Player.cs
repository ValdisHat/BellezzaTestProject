using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody2D
{
	public const float Speed  = 400;
	public const float JumpVelocity  = -700;
	public const float Acceleration = 800;
	private Vector2 velocity = Vector2.Zero;
	
	private AnimatedSprite2D _animatedSprite;
	private AnimationTree _animationTree;
	private AnimationNodeStateMachinePlayback _animationState;
	private string _animationName;



	public override void _Ready()
	{
		
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animationTree = GetNode<AnimationTree>("AnimationTree");
		_animationTree.Active = true;
		_animationState = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");

		
	}
	 public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		_animationName = "";

		bool isJumping = Velocity.Y < 0 && !IsOnFloor();
        bool isFalling = Velocity.Y > 0 && !IsOnFloor();
	
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;	
		}

		if (Input.IsActionJustPressed("space") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		if (Input.IsActionJustReleased("space") && Velocity.Y < 0)
		{
			velocity.Y = JumpVelocity/3;
		}
		
		

		float direction = Input.GetAxis("moveLeft", "moveRight");
		if (direction != 0)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction*Speed, Acceleration*(float)delta);
			_animatedSprite.FlipH = velocity.X < 0; 	
			if (IsOnFloor() && Velocity.Y == 0)
			{
				_animationState.Travel("walk");
			}	
		}
		else
		{
			
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			if (IsOnFloor())
            {
               _animationState.Travel("idle");
            }
		}
		if (isFalling)
		{
			_animationState.Travel("jumpFall");
		}
		if (isJumping)
		{
			_animationState.Travel("jumpStart");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}