using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

public class HeroDragAndDropLogic : Object {

    [Signal]
    public delegate void OnHeroMerge(Node node, Array collided);
    [Signal]
    public delegate void OnHeroStartMerge(Node node);
    [Signal]
    public delegate void OnHeroEndMerge(Node node);
    
    
    private const string DragObjectsGroup = "DragableObject";
    private Vector3 _myLastPos;
    private bool _pressed = false;
    private Spatial Target { get; set; }
    private Area DragableArea { get; set; }

    public HeroDragAndDropLogic() {
        
    }

    public HeroDragAndDropLogic(Spatial target, Area dragableArea) {
        Target = target;
        DragableArea = dragableArea;
        _myLastPos = Target.Translation;

    }


    public void OnDragStart(Node node) {
        _pressed = true;
        _myLastPos = Target.Translation;
        DragableArea.SetCollisionLayerBit(9, true); // activate dragable area
        DragStartAdditionalInstructions();
    }

    public void OnDragStop(Node node) {
        if (!_pressed) return;
        Target.Translation = _myLastPos;
        _pressed = false;
        DragableArea.SetCollisionLayerBit(9, false); // deactivate dragable area
        DragEndAdditionalInstructions();
    }

    public void OnDrag(Node node, Dictionary cast) {
        var pos = (Vector3) cast["position"];
        Target.Translation = new Vector3(pos.x, Target.Translation.y, pos.z);
    }

    private void StopDragAllOtherObjects(Node node) {
        var draggable = Target.GetTree().GetNodesInGroup(DragObjectsGroup);
        DragableObject excludeNode = (DragableObject)node;
        foreach (DragableObject myDragableObject in draggable) {
            GD.Print($"{myDragableObject} {excludeNode}");
            if (myDragableObject != excludeNode) {
                myDragableObject.EmitSignal("DragStop", myDragableObject);
            }
        }
    }

    private void DragStartAdditionalInstructions() {
        HeroesStartMerge();
    }

    private void DragEndAdditionalInstructions() {
        var myCollided = Target.GetNodeOrNull<Area>("PickUpArea")?.GetOverlappingAreas();
        if (myCollided?.Count != 0)
            HeroesMerge(myCollided);
        HeroesEndMerge();
    }

    private void HeroesMerge(Array collided) {
        
        var heroArea = collided[0] as Area;
        var hero = heroArea?.GetParentOrNull<BaseHero>();
        GD.Print($"MERGE_DEBUG: HERO {hero} COLLIDED {collided}");
        if (hero != null) {
            var feedHero = Target as BaseHero;
            if (hero.HeroType == feedHero?.HeroType && hero.Level == feedHero.Level) {
                if (hero.LevelUp())
                    Target.QueueFree();
            }
        }
    }

    private void HeroesStartMerge() {
        var feedHero = Target as BaseHero;
        if (feedHero == null) return;
        foreach (var child in GetMergeGroup()) {
            if (child is BaseHero hero && hero != feedHero) {
                if (hero.HeroType == feedHero?.HeroType && hero.Level == feedHero.Level) {
                    hero.AvailableToMerge = true;
                }
            }
        }
    }

    private void HeroesEndMerge() {
        foreach (var child in GetMergeGroup()) {
            if (child is BaseHero hero) {
                hero.AvailableToMerge = false;
            }
        }
    }

    private Array GetMergeGroup() {
        if (Target is BaseHero hero)
            return hero.GetTree().GetNodesInGroup(hero.myGroupName);
        return new Array();
    }
}