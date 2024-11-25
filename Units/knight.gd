extends CharacterBody2D

# Damage and timing constants
const DAMAGE_INTERVAL = 0.5  # Interval between damage application
const DAMAGE_AMOUNT = 30     # Damage dealt per interval

# Node references
@onready var animation = $AnimationPlayer
@onready var idle = $idle
@onready var attack = $attack
@onready var die = $die
@onready var attack_area = $Area2D

var time = 0.0  # Tracks animation state timing
var enemy_in_range: Node = null  # Tracks the current enemy in range
@onready var attack_timer = Timer.new()  # Timer for controlling damage intervals

func _ready():
	# Attach the attack timer dynamically
	add_child(attack_timer)
	attack_timer.wait_time = DAMAGE_INTERVAL
	attack_timer.connect("timeout", Callable(self, "_apply_damage"))

	# Connect attack area signals
	attack_area.connect("body_entered", Callable(self, "_on_enemy_entered"))
	attack_area.connect("body_exited", Callable(self, "_on_enemy_exited"))

func _physics_process(delta: float) -> void:
	if enemy_in_range:
		_attack_mode()
	else:
		_idle_mode()

# Idle mode: no enemies in range
func _idle_mode():
	die.visible = false
	attack.visible = false
	idle.visible = true
	if not animation.is_playing() or animation.current_animation != "idle":
		animation.play("idle")

# Attack mode: an enemy is in range
func _attack_mode():
	die.visible = false
	idle.visible = false
	attack.visible = true
	if not animation.is_playing() or animation.current_animation != "attack":
		animation.play("attack")

# When an enemy enters the attack range
func _on_enemy_entered(body: Node) -> void:
	if body.is_in_group("enemies"):  # Ensure it's an enemy
		print("Enemy detected: " + body.name)
		enemy_in_range = body
		attack_timer.start()  # Start applying damage

# When an enemy exits the attack range
func _on_enemy_exited(body: Node) -> void:
	if body == enemy_in_range:
		print("Enemy exited range: " + body.name)
		enemy_in_range = null
		attack_timer.stop()  # Stop applying damage

# Apply damage to the enemy
func _apply_damage() -> void:
	if enemy_in_range and attack.visible:
		print("Dealing damage to enemy: " + enemy_in_range.name)
		enemy_in_range.TakeDamage(DAMAGE_AMOUNT)

# Optional: Die mode (not used for now)
func _die_mode():
	idle.visible = false
	attack.visible = false
	die.visible = true
	animation.play("die")
