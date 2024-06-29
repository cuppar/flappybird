extends CanvasLayer

const DEFAULT_MUSIC: String = ""

signal game_entered
signal game_exited
signal scene_changed(old_scene, new_scene)

@onready var color_rect: ColorRect = $ColorRect

func _ready() -> void:
	_on_scene_changed(null, get_tree().current_scene)

func fade_to_black():
	var tween := create_tween()
	tween.tween_callback(color_rect.show)
	tween.tween_property(color_rect, "color:a", 1.0, 0.2)
	await tween.finished

func fade_from_black():
	var tween := create_tween()
	tween.tween_property(color_rect, "color:a", 0.0, 0.3)
	tween.tween_callback(color_rect.hide)
	await tween.finished

func change_scene_to_file(scene_file: String):
	if not scene_file:
		return
	
	await fade_to_black()
	var old_scene := get_tree().current_scene
	old_scene.queue_free()
	var new_scene := load(scene_file).instantiate() as Node
	
	var root := get_tree().root
	root.remove_child(old_scene)
	root.add_child(new_scene)
	get_tree().current_scene = new_scene
	_on_scene_changed(old_scene, new_scene)

	await fade_from_black()
	
func _on_scene_changed(old_scene: Node, new_scene: Node):
	scene_changed.emit(old_scene, new_scene)
	var was_in_game := false
	var is_in_game := false
	
	if old_scene and "scene_translation_info" in old_scene:
		was_in_game = old_scene.scene_translation_info.is_game_scene
	if new_scene and "scene_translation_info" in new_scene:
		is_in_game = new_scene.scene_translation_info.is_game_scene

	if was_in_game != is_in_game:
		if is_in_game:
			game_entered.emit()
		else:
			game_exited.emit()
	
	var music := DEFAULT_MUSIC
	if is_in_game and new_scene.bg_music_override:
		music = new_scene.bg_music_override
	#MusicManager.play(music)

func change_scene_to_file_with_pause(scene_file: String):
	get_tree().paused = true
	change_scene_to_file(scene_file)
	get_tree().paused = false
