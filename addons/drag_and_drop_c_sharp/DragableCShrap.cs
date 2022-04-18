using Godot;
using System;

[Tool]
public class DragableCShrap : EditorPlugin
{
    public override void _EnterTree() {
       
        var texture = GD.Load<Texture>("res://icon.png");
        
        var script = GD.Load<Script>("res://addons/drag_and_drop_c_sharp/DragableObject.cs");
        AddCustomType("DragableObject", "Node", script, texture);
        
        script = GD.Load<Script>("res://addons/drag_and_drop_c_sharp/DragableController.cs");
        AddCustomType("DragableController", "Node", script, texture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("DragableObject");
        RemoveCustomType("DragableController");
    }
}
