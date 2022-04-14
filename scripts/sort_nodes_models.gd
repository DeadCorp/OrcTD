tool
extends Node




export var cols = 10
export var off = 3.0
export var sc = Vector3(2.0, 2.0, 2.0)

func _ready():
	pass # Replace with function body.
func _process(delta):
	if Engine.editor_hint:
		if Input.is_action_just_pressed("ui_up"):
			var pos = Vector3.ZERO
			var ind = 0
			for i in get_children():
				i.transform.origin = pos
				i.scale = sc
				pos += Vector3(off * sc.x, 0, 0)
				if (ind % cols == 0) :
					pos += Vector3(0, 0, off*sc.z)
					pos.x = 0
				ind+=1

		if Input.is_action_just_pressed("ui_down"):
			for i in get_children():
				i.transform.origin = Vector3.ZERO
				i.scale = Vector3.ONE
				
