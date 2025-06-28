using Godot;
using System;

public partial class CheckSimple : Area2D
{
	private Material _originalMaterial;
	private Material _materialCopy;
	public override void _Ready()
	{
		// 连接Area2D的鼠标事件信号
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		InputEvent += OnInputEvent;

				// 创建材质副本
		var canvasItem = GetChild<CanvasItem>(0);
		if (canvasItem != null && canvasItem.Material != null)
		{
			_originalMaterial = canvasItem.Material;
			_materialCopy = _originalMaterial.Duplicate() as Material;
			canvasItem.Material = _materialCopy;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// 鼠标悬停事件
	private void OnMouseEntered()
	{
		// 可以在这里添加悬停效果，比如改变颜色或大小
		if (GetChild<CanvasItem>(0) != null)
		{
			GetChild<CanvasItem>(0).Material.Set("shader_param/Outline", 0.92f);
		}
	}

	// 鼠标离开事件
	private void OnMouseExited()
	{
		// 恢复原始状态
		if (GetChild<CanvasItem>(0) != null)
		{
			GetChild<CanvasItem>(0).Material.Set("shader_param/Outline", 1.0f);
		}
	}

	// 输入事件处理（包括点击）
	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				// 可以在这里添加点击效果
				if (GetChild<Sprite2D>(0) != null)
				{
					GetChild<Sprite2D>(0).Scale = new Vector2(0.9f, 0.9f); // 点击时稍微缩小
				}
			}
			else if (mouseEvent.ButtonIndex == MouseButton.Left && !mouseEvent.Pressed)
			{
				// 鼠标释放时恢复大小
				if (GetChild<Sprite2D>(0) != null)
				{
					GetChild<Sprite2D>(0).Scale = new Vector2(1.0f, 1.0f);
				}

			}
		}
	}
}
