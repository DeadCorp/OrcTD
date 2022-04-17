using Godot;
using System;
using Object = Godot.Object;

public class DragAndDrop : Area {
    [Signal]
    delegate void Clicked();

    [Signal]
    delegate void Released(bool forAll);
    
    private Vector3 _myLastTranslation;
    private bool _pressed = false;
    private Spatial _myParent;

    private Vector2 _clickPoint;

    private ClippedCamera cam;
    public override void _Ready() {
        Connect("Clicked", this, "OnClick");
        Connect("Released", this, "OnReleased");
        
        _myParent = (Spatial) Owner;
        _myLastTranslation = _myParent.Translation;
        
    }

    public override void _PhysicsProcess(float delta) {
        if (_pressed) {
            var mouse_pos = GetViewport().GetMousePosition();
            var ray_origin = cam.ProjectRayOrigin(mouse_pos);
            var ray_direction = cam.ProjectRayNormal(mouse_pos);


            var from = ray_origin;
            var to = ray_origin + ray_direction * 11;
            var space_state = GetWorld().DirectSpaceState;
            var hit = space_state.IntersectRay(from, to, null, 2, false, true);
            if (hit.Count != 0) {
                GD.Print(hit);
                var pos = (Vector3) hit["position"];
                _myParent.Translation = new Vector3(pos.x, pos.y, pos.z);
            }
            
            
        }
    }

    public override void _InputEvent(Object camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx) {
        if (@event is InputEventMouseButton mouseButton) {
            if (mouseButton.Pressed) {
                EmitSignal(nameof(Clicked));
                cam = (ClippedCamera) camera;
            }

            if (!mouseButton.Pressed) {
                EmitSignal(nameof(Released), false);
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event) {
        if (@event is InputEventMouseButton mouseButton) {
            if (!mouseButton.Pressed) {
                EmitSignal(nameof(Released), false);
            }
        }
    }

    public void OnClick() {
        _pressed = true;
        _myLastTranslation = _myParent.Translation;
        GetTree().CallGroup("DragAndDrop", "OnReleased", true);
    }

    public void OnReleased(bool forAll) {
        if ((forAll && !_pressed) || (!forAll && _pressed)) {
            _pressed = false;
            if (_myParent.Translation != _myLastTranslation) _myParent.Translation = _myLastTranslation;
        }
    }
}