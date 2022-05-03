using Godot;
using System;
using Godot.Collections;

[Tool]
public class DragableObject : Node {

	[Signal]
	delegate void DragStart(Node node);

	[Signal]
	delegate void DragStop(Node node);

	[Signal]
	delegate void DragMove(Node node, Dictionary cast);


	private readonly string groupName = "DragableController";

	private DragableController _controller;

	private Node _current;


	public override void _Ready() {
		var controllers = GetTree().GetNodesInGroup(groupName);
		if (controllers.Count == 1)
			_controller = (DragableController) controllers[0];

		if (Engine.EditorHint) {
			SetProcess(false);
			return;
		}

		if (_controller == null) {
			GD.Print("Miss DragableController");
		}
		else {
			var dragable = GetParent();
			//dragable.Connect("mouse_entered", this, "mouse_entered", new Godot.Collections.Array {dragable}); //create methods if need // cary on android - in click point first will be called input_event - after that mouse_entered/exited
			//dragable.Connect("mouse_exited", this, "mouse_exited", new Godot.Collections.Array {dragable}); //create methods if need // cary on android - in click point first will be called input_event - after that mouse_entered/exited
			dragable.Connect("input_event", this, "input_event", new Godot.Collections.Array {dragable});
			
			_controller.InitDraggable(this);
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
			if (mouseButton.Pressed) {
				_current = node.GetParent();
				EmitSignal(nameof(DragStart), this);
			}
			else if (_current != null) {
				EmitSignal(nameof(DragStop), this);
			}
			
		}
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == (int) ButtonList.Left) {
			if (_current != null && !mouseButton.Pressed)
				EmitSignal(nameof(DragStop), this);
		}
	}
	
}
