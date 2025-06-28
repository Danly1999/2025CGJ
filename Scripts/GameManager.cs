using Godot;
using System;

public partial class GameManager
{
    public GodotObject _dialogicBridge;
    private static readonly object _lock = new object();
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameManager();
                    }
                }
            }
            return _instance;
        }
    }
    private GameManager()
    {
        // 加载并实例化桥接脚本
        var bridgeScript = GD.Load<GDScript>("res://Scripts/dialogic_bridge.gd");
        _dialogicBridge = (GodotObject)bridgeScript.New();
    }
    
    // 游戏管理方法示例
    public void InitializeGame()
    {
        GD.Print("游戏初始化完成");
    }
    
    public void PauseGame()
    {
        GD.Print("游戏暂停");
    }
    
    public void ResumeGame()
    {
        GD.Print("游戏恢复");
    }
}
