using Godot;
using System;
using Godot.Collections;
using Object = Godot.Object;

public class HeroDragAndDropLogic : Object {
    private const string DragObjectsGroup = "DragableObject";
    private Vector3 _myLastPos;
    private bool _pressed = false;
    private Vector3 _offset = Vector3.Zero;
    public Spatial Target { get; set; }
    

    public HeroDragAndDropLogic(Spatial target) {
        Target = target;
        _myLastPos = Target.Translation;
    }

    public void OnDragStart(Node node, Vector3 offset) {
        GD.Print($"dragstart {node}");
        _pressed = true;
        _myLastPos = Target.Translation;
        _offset = offset;
    }

    public void OnDragStop(Node node) {
        if (_pressed) {
            GD.Print($"dragstop {node}");
            Target.Translation = _myLastPos;
            _pressed = false;
        }
    }

    public void OnDrag(Node node, Dictionary cast) {
        //GD.Print($"dragmove {node.Name}");
        var pos = (Vector3) cast["position"];
        Target.Translation = new Vector3(pos.x + _offset.x, Target.Translation.y, pos.z - _offset.z);
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
}