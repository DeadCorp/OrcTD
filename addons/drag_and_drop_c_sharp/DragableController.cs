using Godot;
using System;
using Godot.Collections;

[Tool]
public class DragableController : Node {
    private readonly string groupName = "DragableController";

    [Export()] public float RayLenght = 100;
    private Camera camera;
    private DragableObject draging;

    public override void _EnterTree() {
        AddToGroup(groupName, true);
    }

    public override void _Ready() {
	    camera = GetTree().Root.GetCamera();
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

    public void _drag_start(DragableObject node) {
	    draging = node;
	    SetPhysicsProcess(true);
    }
    
    public void _drag_stop(DragableObject node) {
	    draging = null;
	    SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(float delta) {
	    if (draging != null) {
		    var mouse = GetViewport().GetMousePosition();
		    var from = camera.ProjectRayOrigin(mouse);
		    var to = from + camera.ProjectRayNormal(mouse) * RayLenght;
		    
		    Dictionary cast = camera.GetWorld().DirectSpaceState.IntersectRay(from, to, null, 0b1000000000, false, true);
		    if (cast.Count != 0) {
			    draging.on_hover(cast);
		    }
	    }
    }
}