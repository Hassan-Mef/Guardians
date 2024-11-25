extends CharacterBody2D

const SPEED = 300.0
const JUMP_VELOCITY = -400.0
const DAMAGE_INTERVAL = 0.5  # Time in seconds between damage application
const DAMAGE_AMOUNT = 25    # Amount of damage dealt per interval

@onready var animation = $AnimationPlayer
@onready var attack_area = $Area2D
@onready var attack_timer = Timer.new()  # Timer to control damage intervals

var enemy_in_range: Node = null  # Tracks the current enemy in range

func _ready():
	# Add the attack timer as a child and configure it
	add_child(attack_timer)
	attack_timer.wait_time = DAMAGE_INTERVAL
	attack_timer.connect("timeout", Callable(self, "_apply_damage"))

	# Connect the body_entered and body_exited signals of attack_area
	var enter_result = attack_area.connect("body_entered", Callable(self, "_on_enemy_entered"))
	var exit_result = attack_area.connect("body_exited", Callable(self, "_on_enemy_exited"))
	if enter_result == OK and exit_result == OK:
		print("Signals connected successfully.")
	else:
		print("Failed to connect signals.")

func _physics_process(delta: float) -> void:
	if not animation.is_playing():
		animation.play("attack")

# Function to handle when an enemy enters the Area2D
func _on_enemy_entered(body: Node) -> void:
	if body.is_in_group("enemies"):  # Ensure this is an enemy
		print("Enemy detected in range: " + body.name)
		enemy_in_range = body  # Start tracking this enemy
		attack_timer.start()  # Start the attack timer

# Function to handle when an enemy exits the Area2D
func _on_enemy_exited(body: Node) -> void:
	if body == enemy_in_range:
		print("Enemy exited the attack range: " + body.name)
		enemy_in_range = null  # Stop tracking the enemy
		attack_timer.stop()  # Stop the attack timer

# Function to apply damage to the enemy
func _apply_damage() -> void:
	if enemy_in_range:
		print("Dealing damage to enemy: ", enemy_in_range.name)
		enemy_in_range.TakeDamage(DAMAGE_AMOUNT)
