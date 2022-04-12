using Godot;
using System;
[Tool]
public class SortModels : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public int cols = 10;
	[Export]
	public float offset = 3.5f;
	[Export]
	public Vector3 need_scale = new Vector3(2.0f, 2.0f, 2.0f);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	  if (Engine.EditorHint) {
		  if (Input.IsActionJustPressed("ui_up")) {
			  var ind = 0;
			  var pos = new Vector3(0.0f, 0.0f, 0.0f);
			  foreach(var i in GetChildren()) {
				  i.transform = pos;
				  i.scale = need_scale;
				  pos.x += offset * need_scale.x;
				  if (ind % cols == 1) {
					  pos.x = 0.0f;
					  pos.z += offset * need_scale.z;
				  }
				  ind++;

			  }
		  }
		  if (Input.IsActionJustPressed("ui_down")) {
			 foreach(var i in GetChildren()) {
				
				i.transform = new Vector3(0.0f,0.0f,0.0f);
				i.scale = new Vector3(1.0f,1.0f,1.0f);
			 }
		  }
	  }
  }
}
