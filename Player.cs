using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -500.0f;
	public bool isJumping = false;

	private Tween shakeTween;
	private Sprite2D sprite;
	private Vector2 spriteOriginalPosition;
	private Random random = new Random();
	private bool isShaking = false;

	public override void _Ready()
	{
		// 获取 Sprite2D 子节点
		sprite = GetNode<Sprite2D>("Sprite2D");
		spriteOriginalPosition = sprite.Position;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("shake"))
		{
			if (!isShaking)
			{
				isShaking = true;
				StartRandomShake();
			}
		}
		else
		{
			if (isShaking)
			{
				isShaking = false;
				if (shakeTween != null && shakeTween.IsRunning())
				{
					shakeTween.Kill();
				}
				sprite.Position = spriteOriginalPosition;
			}
		}
	}

	private void StartRandomShake()
	{
		if (!isShaking) return;

		// 随机生成一个偏移量
		float shakeAmount = 3f;
		float offsetX = (float)(random.NextDouble() * 2 - 1) * shakeAmount;
		float offsetY = (float)(random.NextDouble() * 2 - 1) * shakeAmount;

		Vector2 targetPos = spriteOriginalPosition + new Vector2(offsetX, offsetY);

		// 创建Tween
		shakeTween = CreateTween();
		float shakeTime = 0.01f; // 0.04~0.1秒之间

		shakeTween.TweenProperty(sprite, "position", targetPos, shakeTime)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		// Tween结束后回调自己
		shakeTween.Finished += StartRandomShake;
	}
 	public void Jump()
	{
		Velocity = new Vector2(Velocity.X, JumpVelocity);
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		if (direction.X < 0)
		{
			sprite.Scale = new Vector2(-Mathf.Abs(sprite.Scale.X), sprite.Scale.Y);
		}
		else if (direction.X > 0)
		{
			sprite.Scale = new Vector2(Mathf.Abs(sprite.Scale.X), sprite.Scale.Y);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

}
