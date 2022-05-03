using Godot;
using System;
using Object = Godot.Object;

public class HeroCombatLogic : Object
{
    private readonly BaseHero _parent;
    private Area _area;
    

    public HeroCombatLogic() {
    }

    public HeroCombatLogic(BaseHero target) {
        _parent = target;

        _area = target.GetNode<Area>("EnemyDetectArea");
        _area.Connect("area_entered", this, "OnEnemyEnter");
        _area.Connect("area_exited", this, "OnEnemyExit");
        GD.Print($"{_area.IsConnected("area_entered", this, "OnEnemyEnter")}");
        var child = (CylinderShape)_area.GetChild<CollisionShape>(0).Shape;
        child.Radius = _parent.AttackRange;
    
    }

    public void OnEnemyEnter() {
        
    }
    public void OnEnemyExit() {
        
    }

    

}