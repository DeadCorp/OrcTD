using Godot;
using System;
using System.Linq;

public class BadGuysSpawner : Spatial {

    [Export()] public int EnemyCount = 1;
    [Export()] public float TimerTime = 0.2f;

    private int _timerCallMax;
    private int _timerCalled = 0;
    private Vector3[] _positions;
    private PackedScene _badGuy;
    
    public override void _Ready() {
        _timerCallMax = EnemyCount;
        var timer = GetNode<Timer>("Timer");
        timer.WaitTime = TimerTime;
        var childPath = GetNode<Spatial>("PathPoints");
        _positions = new Vector3[childPath.GetChildren().Count];
        var index = 0;
        foreach (Position3D child in childPath.GetChildren()) {
            if (child != null)
                _positions[index++] = child.GlobalTransform.origin;
        }

        _badGuy = (PackedScene)GD.Load("res://scenes/test/BadGuys/BadGuy.tscn") ;
    }

    public void _on_Timer_timeout() {
        if (_timerCalled < _timerCallMax) {
            _timerCalled++;
            BaseEnemy badGuy = (BaseEnemy)_badGuy.Instance();
            badGuy.PathInit(_positions);
            AddChild(badGuy);
        }
        
        
    }
}