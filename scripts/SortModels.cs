using Godot;
using System;
[Tool]
public class SortModels : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      if (Engine.EditorHint) {
          if (Input.IsActionJustPressed("ui_up")) {
              GD.Print("ALO");
          }
      }
  }
}
