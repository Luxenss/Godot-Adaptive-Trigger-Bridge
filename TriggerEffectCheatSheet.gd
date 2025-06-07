extends Node

func _ready() -> void:
	Trigger.reset_triggers_on_quit = true  # Default true
	Trigger.error_on_invalid_index = false # Default false
	Trigger.print_on_effect_apply = false  # Default false

	print(Trigger.HasDualSense())
	print(Trigger.GetConnectedDualSenseInfos())

	Trigger.Off("l")
	Trigger.Off("r")
	
	# position ∈ [0, 9], strength ∈ [0, 8]
	Trigger.Feedback("r", 3, 6)

	# startPosition ∈ [2, 7], endPosition ∈ [startPosition+1, 8], strength ∈ [0, 8]
	Trigger.Weapon("r", 2, 4, 7)

	# position ∈ [0, 9], amplitude ∈ [0, 8], frequency ∈ [1, 16]  it’s recommended not to exceed 16 too much for frequency.
	Trigger.Vibration("l", 5, 7, 7)

	# strength: Array[10] of values ∈ [0, 8]
	Trigger.MultiplePositionFeedback("r", [0, 0, 3, 4, 5, 4, 3, 0, 0, 0])
	
	# startPosition ∈ [0, 8], endPosition ∈ [startPosition+1, 9], startStrength, endStrength ∈ [1, 8]
	Trigger.SlopeFeedback("l", 0, 7, 2, 6)

	# amplitude: Array[10] of values ∈ [0, 8], frequency ∈ [1, 16]  it’s recommended not to exceed 16 too much for frequency.
	Trigger.MultiplePositionVibration("r", 12, [0, 1, 2, 3, 4, 3, 2, 1, 0, 0])

	# startPosition ∈ [0, 8], endPosition ∈ [startPosition+1, 8], strength, snapForce ∈ [0, 8]
	Trigger.Bow("l", 3, 6, 5, 4)

	# startPosition ∈ [0, 8], endPosition ∈ [startPosition+1, 9], firstFoot ∈ [0, 6], secondFoot ∈ [firstFoot+1, 7], frequency ∈ [1, 16]  it’s recommended not to exceed 16 too much for frequency.
	Trigger.Galloping("r", 1, 7, 2, 4, 8)

	# startPosition ∈ [0, 8], endPosition ∈ [startPosition, 9], amplitudeA, amplitudeB ∈ [0, 7], frequency ∈ [1, 16]  it’s recommended not to exceed 16 too much for frequency.
	Trigger.Machine("l", 2, 9, 3, 6, 10, 4)
