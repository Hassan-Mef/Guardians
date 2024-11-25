extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0
@onready var animation=$AnimationPlayer


func _physics_process(delta: float) -> void:
	if not animation.is_playing():
		print("only 1")
		animation.play("attack")



func _on_animation_player_animation_finished(anim_name: StringName) -> void:
	print("enter on end")
	if (anim_name=="attack"):
		print("attack animation is ended")
