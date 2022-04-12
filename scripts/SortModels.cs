using Godot;
using System;
[Tool]
public class SortModels : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public int Cols = 10;
	[Export]
	public float Offset = 3.5f;
	[Export]
	public Vector3 NeedScale = new Vector3(2.0f, 2.0f, 2.0f);
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
			  foreach(Spatial i in GetChildren()) {
				 
				  i.Translation = pos;
				  i.Scale = NeedScale;
				  pos.x += Offset * NeedScale.x;
				  if (ind % Cols == 0) {
					  pos.x = 0.0f;
					  pos.z += Offset * NeedScale.z;
				  }
				  ind++;

			  }
		  }
		  if (Input.IsActionJustPressed("ui_down")) {
			 foreach(Spatial i in GetChildren()) {
				
				i.Translation = new Vector3(0.0f,0.0f,0.0f);
				i.Scale = new Vector3(1.0f,1.0f,1.0f);
			 }
		  }
	  }
  }
}
