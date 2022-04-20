using Godot;
using System;
using Godot.Collections;

[Tool]
public class DragableController : Node {
    private readonly string groupName = "DragableController";

    [Export()] public float RayLenght = 100;
    private Camera _camera;
    private DragableObject _draging;

    public override void _EnterTree() {
        AddToGroup(groupName, true);
    }

    public override void _Ready() {
	    _camera = GetTree().Root.GetCamera();
	    SetPhysicsProcess(false);
    }

    public override string _GetConfigurationWarning() {
        if (GetTree().GetNodesInGroup(groupName).Count > 1)
            return "Must be only one controller";
        return "";
    }

    public void InitDraggable(Node node) {
	    node.Connect("DragStart", this, "_drag_start");
	    node.Connect("DragStop", this, "_drag_stop");
    }

    public void _drag_start(DragableObject node, Vector3 offset) {
	    _draging = node;
	    SetPhysicsProcess(true);
    }
    
    public void _drag_stop(DragableObject node) {
	    _draging = null;
	    SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(float delta) {
	    if (_draging != null) {
		    var mouse = GetViewport().GetMousePosition();
		    var from = _camera.ProjectRayOrigin(mouse);
		    var to = from + _camera.ProjectRayNormal(mouse) * RayLenght;
		    
		    Dictionary cast = _camera.GetWorld().DirectSpaceState.IntersectRay(from, to, null, 0b1000000000, false, true);
		    if (cast.Count != 0) {
			    _draging.on_hover(cast);
		    }
	    }
    }
}