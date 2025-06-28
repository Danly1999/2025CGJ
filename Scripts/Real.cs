using Godot;
using System;

public partial class Real : Node2D
{
        public bool over = false;
    public override void _Process(double delta)
    {
        if(!over)
        {
            return;
        }
        GetNode<Node2D>("../leibi").Scale = new Vector2(2,2);
        foreach (Node node in GetChildren())
        {
            // 检查是否为 Node2D 类型
            if (node is Node2D node2d)
            {
                try
                {
                    if(node.GetNode<CpuParticles2D>("CPUParticles2D") != null)
                    {
                        node.GetNode<CpuParticles2D>("CPUParticles2D").Emitting = true;
                    }
                }
                catch (System.Exception)
                {
                    
                }

                
                // 获取局部Y轴方向（Transform2D的Y轴）
                Vector2 localYDirection = node2d.Transform.Y;
                
                // 计算移动速度
                float speed = Random.Shared.Next(50, 300) * (float)delta;
                
                // 沿着局部Y轴方向移动
                Vector2 currentPos = node2d.Position;
                Vector2 newPos = currentPos + localYDirection * speed;
                
                node2d.Position = newPos;
                

            }
        }
        
    }
}
