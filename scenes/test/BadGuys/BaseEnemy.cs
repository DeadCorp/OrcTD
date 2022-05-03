using Godot;
using System;
using Array = Godot.Collections.Array;

public class BaseEnemy : Spatial {
   
    private int _pathNode;
    private Vector3[] _pathPoints = new Vector3[]{};

    [Export()] public float Speed = 5f;

    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta) {
        if (_pathPoints.Length != 0) {
            if (_pathNode < _pathPoints.Length) {
                var direction = (_pathPoints[_pathNode] - GlobalTransform.origin);
                var speedFrame = Speed * delta;
                if (direction.Length() < speedFrame) {
                    _pathNode++;
                }
                else {
                    Translate(direction.Normalized() * speedFrame);
                }
            }
            else {
                PathEndMove();
            }
        }
    }

    public void PathInit(Vector3[] positions) {
        _pathPoints = positions;
        PathStartMove();
         
    }
    public void PathStartMove() {
        _pathNode = 0;
    }

    public void PathEndMove() {
        GD.Print("yea i kill");
        _pathPoints = new Vector3[] { };
        _pathNode = 0;
        QueueFree();
    }
    
}