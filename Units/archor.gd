extends CharacterBody2D

const DAMAGE_INTERVAL = 0.5  # Time in seconds between damage application
const DAMAGE_AMOUNT = 25   # Amount of damage dealt per interval

@onready var animation = $AnimationPlayer
@onready var attack_area = $Area2D
@onready var attack_timer = Timer.new()  # Timer to control damage intervals
@export var arrow_scene: PackedScene = preload("res://Units/arrow.tscn")  # Arrow scene for archers
var enemy_in_range: Node = null  # Tracks the current enemy in range
var archerPlay: bool = false  # Don't start attack animation unless enemy is in range
var arrow: PackedScene = null

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
	# Play the attack animation if there is an enemy in range
	if archerPlay and enemy_in_range:
		animation.play("attack")  # Play the attack animation
	elif not archerPlay:  # Otherwise, play idle animation
		animation.play("Idle")

# Function to handle when an enemy enters the Area2D
func _on_enemy_entered(body: Node) -> void:
	if body.is_in_group("enemies"):  # Ensure this is an enemy
		print("Enemy detected in range: " + body.name)
		enemy_in_range = body  # Start tracking this enemy
		archerPlay = true  # Enable attack animation
		attack_timer.start()  # Start the attack timer

# Function to handle when an enemy exits the Area2D
func _on_enemy_exited(body: Node) -> void:
	if body == enemy_in_range:
		print("Enemy exited the attack range: " + body.name)
		enemy_in_range = null  # Stop tracking the enemy
		attack_timer.stop()  # Stop the attack timer
		archerPlay = false  # Disable attack animation

# Function to apply damage to the enemy
func _apply_damage() -> void:
	if enemy_in_range:
		print("Dealing damage to enemy: ", enemy_in_range.name)
		enemy_in_range.TakeDamage(DAMAGE_AMOUNT)

# Function to handle animation finishing and spawning an arrow
func _on_animation_player_animation_finished(anim_name: StringName) -> void:
	if anim_name == "attack":
		print("Attack animation ended")
		arrow = arrow_scene
		if arrow and enemy_in_range:
			var arrow_instance = arrow.instantiate()
			arrow_instance.position.x = position.x + 54
			arrow_instance.position.y = position.y + 20
			# Calculate direction to the enemy
			var direction = (enemy_in_range.position - arrow_instance.position).normalized()
			arrow_instance.set("direction", direction)  # Store direction in the arrow instance
			get_parent().get_node("/root/Game/Arrows").add_child(arrow_instance)

		# Check if there are still enemies in range, continue attack animation if so
		if enemy_in_range:
			archerPlay = true  # Keep playing the attack animation
