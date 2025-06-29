using Godot;
using System;

public partial class IconDrag : TextureRect
{
    [Export]
    public bool canDelete = false;
    private bool isDragging = false;
    private Vector2 dragOffset;
    private Vector2 originalPosition;
    private IconDrag dragInstance; // 拖拽时创建的实例
    
    public override void _Ready()
    {
        // 确保可以接收输入
        MouseFilter = MouseFilterEnum.Stop;
        
        // 连接鼠标输入信号
        GuiInput += OnGuiInput;
    }
    
    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (mouseButton.Pressed)
                {
                    // 开始拖拽
                    StartDrag(mouseButton.Position);
                }
                else
                {
                    // 结束拖拽
                    StopDrag();
                }
            }
        }
        else if (@event is InputEventMouseMotion mouseMotion && isDragging)
        {
            // 更新拖拽位置
            UpdateDragPosition(mouseMotion.Position);
        }
    }
    
    private void StartDrag(Vector2 mousePosition)
    {
        isDragging = true;
        originalPosition = Position;
        
        // 计算鼠标相对于图标的偏移量
        dragOffset = mousePosition;
        
        // 创建拖拽实例
        CreateDragInstance();
        
        // 改变鼠标光标样式
        Input.SetDefaultCursorShape(Input.CursorShape.Drag);
    }
    
    private void CreateDragInstance()
    {
        // 创建拖拽实例
        dragInstance = new IconDrag();
        
        // 复制原图标的所有属性
        dragInstance.Texture = this.Texture;
        dragInstance.CustomMinimumSize = this.CustomMinimumSize;
        dragInstance.ExpandMode = this.ExpandMode;
        dragInstance.StretchMode = this.StretchMode;
        
        // 设置半透明效果
        dragInstance.Modulate = new Color(1, 1, 1, 0.6f);
        
        // 禁用拖拽实例的输入处理，避免递归
        dragInstance.MouseFilter = MouseFilterEnum.Ignore;
        
        // 将拖拽实例添加到根节点下
        GetParent().AddChild(dragInstance);
        
        // 设置拖拽实例的初始位置
        dragInstance.Position = Position;
    }
    
    private void StopDrag()
    {
        isDragging = false;
        
        // 移除拖拽实例
        
        // 恢复默认鼠标光标
        Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
        
        if (canDelete)
        OnDragEnded();

        RemoveDragInstance();
    }
    
    private void RemoveDragInstance()
    {
        if (dragInstance != null && IsInstanceValid(dragInstance))
        {
            dragInstance.QueueFree();
            dragInstance = null;
        }
    }
    
    private void UpdateDragPosition(Vector2 mousePosition)
    {
        if (!isDragging || dragInstance == null) return;
        
        // 计算新位置
        Vector2 newPosition = GetGlobalMousePosition() - dragOffset;
        
        // 更新拖拽实例的位置（不是原图标的位置）
        dragInstance.Position = newPosition;
        
        // 可选：在这里添加拖拽过程中的逻辑
        OnDragMoved(newPosition);
    }
    
    // 可重写的方法，用于自定义拖拽行为
    protected virtual void OnDragEnded()
    {
        // 检查是否拖拽到删除区域
        CheckForDeleteZone();
        
        // 子类可以重写此方法来添加自定义逻辑
        GD.Print("拖拽结束");
    }
    
    private void CheckForDeleteZone()
    {
        if (dragInstance == null || !IsInstanceValid(dragInstance)) return;
        
        // 获取拖拽实例的最终位置
        Vector2 endPosition = dragInstance.GlobalPosition;
        
        // 定义删除区域（可以根据需要调整）
        // 例如：屏幕右下角区域
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        Rect2 deleteZone = new Rect2(screenSize.X - 500, 0, 500, 500);
        
            GD.Print(endPosition);
        // 检查是否在删除区域内
        if (deleteZone.HasPoint(endPosition))
        {
            GD.Print("666");
            GameManager.Instance._dialogicBridge.Call("start_timeline", "unityend");
            GameManager.Instance._dialogicBridge.Call("connect_signal", this, nameof(DelayedFree));
            Visible = false;
        }
    }
    public async void DelayedFree()
	{
		await ToSignal(GetTree().CreateTimer(2f), "timeout");
		GD.Print("777");
		var scene = GD.Load<PackedScene>("res://Scene/结束.tscn");
		if (scene == null)
		{
			GD.Print("场景加载失败: ", "res://Scene/结束.tscn");
			return;
		}
		
        var node = GetTree().Root.GetNode<Node2D>("Base/Node2D");
		node.AddChild(scene.Instantiate());
		GetParent<Control>().QueueFree();
        GD.Print(GetParent<Control>());
	}
    protected virtual void OnDragMoved(Vector2 newPosition)
    {
        // 子类可以重写此方法来添加自定义逻辑
        // GD.Print($"拖拽到位置: {newPosition}");
    }
    
    // 公共方法，用于外部控制拖拽状态
    public void SetDraggable(bool draggable)
    {
        MouseFilter = draggable ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
    }
    
    public bool IsDragging()
    {
        return isDragging;
    }
    
    public void ResetPosition()
    {
        Position = originalPosition;
    }
    
    // 获取拖拽实例的最终位置（用于确定放置位置）
    public Vector2 GetDragEndPosition()
    {
        if (dragInstance != null && IsInstanceValid(dragInstance))
        {
            return dragInstance.Position;
        }
        return Position;
    }
}
