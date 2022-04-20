using Godot;
using System;
using Godot.Collections;

[Tool]
public class DragableObject : Node {

	[Signal]
	delegate void DragStart(Node node, Vector3 offset);

	[Signal]
	delegate void DragStop(Node node);

	[Signal]
	delegate void DragMove(Node node, Dictionary cast);


	private readonly string groupName = "DragableController";

	private DragableController controller;

	private Node current;


	public override void _Ready() {
		var controllers = GetTree().GetNodesInGroup(groupName);
		if (controllers.Count == 1)
			controller = (DragableController) controllers[0];

		if (Engine.EditorHint) {
			SetProcess(false);
			return;
		}

		if (controller == null) {
			GD.Print("Miss DragableController");
		}
		else {
			var dragable = GetParent();
			//dragable.Connect("mouse_entered", this, "mouse_entered", new Godot.Collections.Array {dragable}); //create methods if need // cary on android - in click point first will be called input_event - after that mouse_entered/exited
			//dragable.Connect("mouse_exited", this, "mouse_exited", new Godot.Collections.Array {dragable}); //create methods if need // cary on android - in click point first will be called input_event - after that mouse_entered/exited
			dragable.Connect("input_event", this, "input_event", new Godot.Collections.Array {dragable});
			
			controller.InitDraggable(this);
		}

	}

	public override string _GetConfigurationWarning() {

		if (!(GetParent() is CollisionObject)) {
			return "Not under a collision object";
		}

		return "";
	}
	
	public void on_hover(Dictionary cast) {
		EmitSignal(nameof(DragMove), this, cast);
	}

	public void input_event(Camera camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal,
		Shape shapeIdx, Node node) {
		
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == (int) ButtonList.Left) {
			GD.Print("tap");
			if (mouseButton.Pressed) {
				current = node.GetParent();
				var spatial = (Spatial) current;
				var offset = clickPosition - spatial.Translation;
				EmitSignal(nameof(DragStart), this, offset);
			}
			else if (current != null) {
				EmitSignal(nameof(DragStop), this);
			}
			
		}
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == (int) ButtonList.Left) {
			if (current != null && !mouseButton.Pressed)
				EmitSignal(nameof(DragStop), this);
		}
	}
	
}
