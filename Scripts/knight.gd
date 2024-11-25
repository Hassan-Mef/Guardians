extends CharacterBody2D


#const SPEED = 300.0
#const JUMP_VELOCITY = -400.0
var time=0
@onready var animation=$AnimationPlayer
@onready var idle=$idle
@onready var attack=$attack
@onready var die=$die

func _physics_process(delta: float) -> void:
	# Add the gravity.
	time+=delta
	if time<=5:
		die.visible=false
		attack.visible=false
		idle.visible=true
		animation.play("idle")
	elif time<=10:
		die.visible=false
		idle.visible=false
		attack.visible=true
		animation.play("attack")
	elif time<=11:
		idle.visible=false
		attack.visible=false
		die.visible=true
		animation.play("die")
	else:
		animation.stop()
		time=0
	#if not is_on_floor():
		#velocity += get_gravity() * delta
#
	## Handle jump.
	#if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		#velocity.y = JUMP_VELOCITY
#
	## Get the input direction and handle the movement/deceleration.
	## As good practice, you should replace UI actions with custom gameplay actions.
	#var direction := Input.get_axis("ui_left", "ui_right")
	#if direction:
		#velocity.x = direction * SPEED
	#else:
		#velocity.x = move_toward(velocity.x, 0, SPEED)
#
	#move_and_slide()
