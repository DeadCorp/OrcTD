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
    public Spatial Target { get; set; }
    public Area DragableArea { get; set; }

    public HeroDragAndDropLogic() {
        
    }
    public HeroDragAndDropLogic(Spatial target,  Area dragableArea, Node mergeHandler) {
        Target = target;
        DragableArea = dragableArea;
        _myLastPos = Target.Translation;
        
        Connect("OnHeroMerge", mergeHandler, "OnHeroesMerge");
        Connect("OnHeroStartMerge", mergeHandler, "OnHeroesStartMerge");
        Connect("OnHeroEndMerge", mergeHandler, "OnHeroesEndMerge");
    }

    public void OnDragStart(Node node) {
        //GD.Print($"dragstart {node}");
        _pressed = true;
        _myLastPos = Target.Translation;
        DragableArea.SetCollisionLayerBit(9, true); // activate dragable area
        DragStartAdditionalInstructions();
    }

    public void OnDragStop(Node node) {
        if (_pressed) { 
            //GD.Print($"dragstop {node}");
            Target.Translation = _myLastPos;
            _pressed = false;
            DragableArea.SetCollisionLayerBit(9, false); // deactivate dragable area
            DragEndAdditionalInstructions();
        }
    }

    public void OnDrag(Node node, Dictionary cast) {
        //GD.Print($"dragmove {node.Name}");
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
        EmitSignal(nameof(OnHeroStartMerge), Target);
    } private void DragEndAdditionalInstructions() {
        var myCollided = Target.GetNodeOrNull<Area>("Area")?.GetOverlappingAreas();
        if (myCollided?.Count != 0)
            EmitSignal(nameof(OnHeroMerge), Target, myCollided);
        EmitSignal(nameof(OnHeroEndMerge), Target);
    }
}