using Godot;
using System;

public partial class Dmove : Node2D
{
    public bool over = false;
    public override void _Process(double delta)
    {
        if(!over)
        {
            return;
        }
        foreach (Node node in GetChildren())
        {
            // 检查是否为 Node2D 类型
            if (node is Node2D node2d)
            {
                // 使用更平滑的移动
                Vector2 currentPos = node2d.Position;
                Vector2 newPos = new Vector2(
                    currentPos.X + Random.Shared.Next(-50, 60)*(float)delta, 
                    currentPos.Y + Random.Shared.Next(-50, 60)*(float)delta
                );
                node2d.Position = newPos;
                
                // 安全地设置透明度
                if (node2d is CanvasItem canvasItem)
                {
                    canvasItem.Modulate = canvasItem.Modulate.Lerp(new Color(1, 1, 1, 0.2f), (float)delta);
                }
            }
        }
        GetNode<Node2D>("../Area2D/bardan").Scale = new Vector2(2, 2);
        
    }
}
