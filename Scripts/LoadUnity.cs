using Godot;
using System;

public partial class LoadUnity : Node2D
{
    public override void _Ready()
    {
        var scene = GD.Load<PackedScene>("res://Scene/win.tscn");
        var canvasLayer = GetTree().Root.GetNode<CanvasLayer>("Base/CanvasLayer");
        canvasLayer.AddChild(scene.Instantiate());
    }
}
