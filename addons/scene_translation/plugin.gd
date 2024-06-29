@tool
extends EditorPlugin

func _enter_tree() -> void:
	# Initialization of the plugin goes here.
	add_autoload_singleton("SceneTranslation", "scene_translation.tscn")
	add_custom_type("SceneTranslationInfo", "Node2D", preload("res://addons/scene_translation/scene_translation_info.gd"), null)

func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	remove_autoload_singleton("SceneTranslation")
	remove_custom_type("SceneTranslationInfo")
