extends Node2D

@export var dialogic_timeline: String = "timeline"
func _ready() -> void:
	if not Dialogic.start(dialogic_timeline):
		push_error("Dialogic 时间线加载失败: ", dialogic_timeline)
	Dialogic.timeline_ended.connect(Over)
	

func Over() -> void:
	var son = get_child(0) as CanvasItem
	print(son)
	son.visible = true
	

func _exit_tree():
	if Dialogic.timeline_ended.is_connected(Over):
		Dialogic.timeline_ended.disconnect(Over)
