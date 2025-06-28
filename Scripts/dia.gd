extends Node2D

@export var dialogic_timeline: String = "timeline"
@export_file("*.tscn") var next_scene_path: String = "res://Scene/小龙虾.tscn"
@export var change_scene: bool = true
func _ready() -> void:
	if not Dialogic.start(dialogic_timeline):
		push_error("Dialogic 时间线加载失败: ", dialogic_timeline)
	if (change_scene):
		Dialogic.timeline_ended.connect(Over)
	return

func Over() -> void:
	var scene = load(next_scene_path)
	if not scene:
		push_error("场景加载失败: ", next_scene_path)
		return
	
	var parent = get_parent()
	if not parent:
		push_error("父节点无效")
		return
	
	parent.add_child(scene.instantiate())
	queue_free()

func _exit_tree():
	if Dialogic.timeline_ended.is_connected(Over):
		Dialogic.timeline_ended.disconnect(Over)
