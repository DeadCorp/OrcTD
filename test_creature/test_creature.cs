using Godot;
using System;

public class test_creature : KinematicBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _stateMachine;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _animationTree = (AnimationTree) GetNode("AnimationTree");
        _stateMachine = (AnimationNodeStateMachinePlayback) _animationTree.Get("parameters/playback");
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta) {
      if (Input.IsActionJustPressed("ui_1")) {
          _stateMachine.Travel("Idle");
      }
      if (Input.IsActionJustPressed("ui_2")) {
          _stateMachine.Travel("Walk");
      }
      if (Input.IsActionJustPressed("ui_3")) {
          _stateMachine.Travel("Run");
      }
  }
}
