extends Node

# 暴露 Dialogic 功能给 C#
func start_timeline(timeline_name: String) -> void:
	Dialogic.start(timeline_name)

func connect_signal(target: Object, method: String) -> void:
	Dialogic.timeline_ended.connect(target.call.bind(method))
