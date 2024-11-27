extends Node2D

var arrow_speed = 800  # Speed of the arrow
var direction: Vector2 = Vector2.ZERO  # The direction the arrow will move in
var damage: int = 10  # Arrow damage value

func _ready():
	# The arrow's initial direction should be set here if it's not set from the Archer script
	if direction == Vector2.ZERO:
		direction = Vector2.RIGHT  # Default direction, modify this if needed

func _physics_process(delta: float) -> void:
	position += direction * arrow_speed * delta  # Move the arrow based on direction and speed
	
	# If the arrow goes off-screen, remove it from the scene
	if not get_viewport_rect().has_point(position):  # Check if the arrow is off-screen
		queue_free()  # Remove the arrow if it goes off-screen

# Collision detection - when the arrow hits an enemy
func _on_area2d_body_entered(body: Node) -> void:
	if body.is_in_group("enemies"):  # Ensure it is an enemy
		body.TakeDamage(damage)  # Apply damage to the enemy
		queue_free()  # Destroy the arrow immediately after hitting the enemy
