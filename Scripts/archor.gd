extends CharacterBody2D

#const SPEED = 300.0
#const JUMP_VELOCITY = -400.0
@onready var animation=$AnimationPlayer
var archerPlay:bool=true

@export var arrow_scene:PackedScene = preload("res://Units/arrow.tscn")
var arrow:PackedScene = null

func _physics_process(delta: float) -> void:
	if archerPlay:
		animation.play("attack")



func _on_animation_player_animation_finished(anim_name: StringName) -> void:
	if (anim_name=="attack"):
		archerPlay=false
		print("attack animation is ended")
		arrow=arrow_scene
		if(arrow):
			var arrow_instance = arrow.instantiate()
			arrow_instance.position.x=position.x+54
			arrow_instance.position.y=position.y+20
			get_parent().get_node("/root/Game/Arrows").add_child(arrow_instance)
