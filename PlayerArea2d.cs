using Godot;
using System;

public partial class PlayerArea2d : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaBodyEntered;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    
	private void OnAreaBodyEntered(Area2D body)
	{
		Node parent = body.GetParent();
		Player player = this.GetParent() as Player;
		player.Jump();
		if (parent != null)
		{
			parent.QueueFree();
		}
	}

}
