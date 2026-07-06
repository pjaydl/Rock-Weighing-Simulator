using UnityEngine;                    // Provides access to Unity's core engine classes
using UnityEngine.InputSystem;        // Enables use of the new Input System (Mouse.current, Keyboard.current, etc.)

// RequireComponent automatically adds a Rigidbody component to the GameObject
// if one does not already exist. This prevents runtime errors because this
// script depends on a Rigidbody for enabling/disabling physics while dragging.
[RequireComponent(typeof(Rigidbody))]
public class DraggableRock : MonoBehaviour
{
    // Reference to the Rigidbody attached to this rock.
    // Used for controlling physics properties such as gravity,
    // movement, and collision behavior.
    private Rigidbody rb;

    // Stores a reference to the scene's Main Camera.
    // The camera is used to convert the mouse position on the screen
    // into a ray that extends into the 3D world.
    private Camera mainCamera;

    // An invisible mathematical plane used as the surface where
    // the rock will move while dragging.
    //
    // Instead of moving freely in 3D space, the mouse ray intersects
    // this plane to calculate the rock's new position.
    private Plane dragPlane;

    // Stores the distance between the point where the user clicked
    // and the object's pivot.
    //
    // Without this offset, the object would instantly snap so that
    // its center aligns exactly with the mouse cursor.
    private Vector3 dragOffset;

    // Indicates whether this rock is currently being dragged.
    //
    // true  = dragging is active
    // false = normal physics simulation
    private bool isDragging;

    // Stores the rock's original Z coordinate.
    //
    // This keeps the rock moving only along the X and Y axes,
    // preventing it from moving closer or farther from the camera.
    private float fixedZPosition;

    // Awake() is called once when the GameObject is created,
    // before Start() and before the first frame.
    private void Awake()
    {
        // Retrieves the Rigidbody attached to the same GameObject.
        rb = GetComponent<Rigidbody>();

        // Retrieves the camera tagged as "MainCamera".
        // This camera will later be used for raycasting.
        mainCamera = Camera.main;
    }

    // Update() runs once every rendered frame.
    // It is ideal for reading player input because input is frame-based.
    private void Update()
    {
        // Safety check.
        //
        // If no mouse device exists (for example on unsupported platforms),
        // stop executing the remainder of Update().
        if (Mouse.current == null)
            return;

        // Detects the exact frame the left mouse button was pressed.
        //
        // wasPressedThisFrame becomes true only once when the button
        // transitions from released to pressed.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Cast a ray from the mouse cursor into the world.
            // If a DraggableRock was hit...
            if (TryGetRockUnderMouse(out DraggableRock hitRock) && hitRock == this)
            {
                // ...begin dragging THIS rock.
                //
                // The "hitRock == this" condition ensures that only
                // the clicked rock starts dragging.
                StartDrag();
            }
        }

        // If dragging is active but the player releases the left mouse button,
        // stop dragging and restore physics.
        if (isDragging && !Mouse.current.leftButton.isPressed)
        {
            EndDrag();
        }
    }

    // FixedUpdate() executes at fixed time intervals.
    //
    // Physics calculations should be performed here because Unity's
    // physics engine updates during FixedUpdate().
    private void FixedUpdate()
    {
        // Continuously update the rock's position while dragging.
        if (isDragging)
        {
            Drag();
        }
    }

    // Attempts to determine whether the mouse cursor is pointing
    // at a GameObject containing a DraggableRock component.
    //
    // Returns:
    // true  -> a draggable rock was found
    // false -> nothing draggable was clicked
    private bool TryGetRockUnderMouse(out DraggableRock hitRock)
    {
        // Initialize the output variable as null.
        hitRock = null;

        // Convert the current mouse position on the screen
        // into a ray extending into the 3D world.
        //
        // The ray starts at the camera and travels forward
        // through the mouse cursor.
        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        // Fire the ray into the scene.
        //
        // Parameters:
        // ray                           -> ray to cast
        // out RaycastHit hit            -> stores hit information
        // Mathf.Infinity                -> unlimited distance
        // Physics.DefaultRaycastLayers  -> collide with default layers
        // Ignore triggers               -> ignore trigger colliders
        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            // Attempt to locate the DraggableRock component
            // on the hit object or one of its parents.
            //
            // GetComponentInParent() is useful if the collider
            // is placed on a child object.
            hitRock = hit.collider.GetComponentInParent<DraggableRock>();
        }

        // Return true if a valid DraggableRock was found.
        return hitRock != null;
    }

    // Called once when dragging begins.
    //
    // Prepares the rock by disabling physics and calculating
    // the information required for smooth movement.
    private void StartDrag()
    {
        // Mark this object as currently being dragged.
        isDragging = true;

        // Remove any existing movement before dragging starts.
        //
        // Prevents the rock from continuing to slide due to momentum.
        rb.linearVelocity = Vector3.zero;

        // Stop any rotational movement.
        rb.angularVelocity = Vector3.zero;

        // Disable physics simulation.
        //
        // The object will now move directly through transform.position.
        rb.isKinematic = true;

        // Disable gravity while dragging.
        rb.useGravity = false;

        // Save the rock's current depth.
        //
        // This value will remain constant throughout dragging.
        fixedZPosition = transform.position.z;

        // Create an invisible drag plane.
        //
        // The plane faces the same direction as the camera
        // and passes through the rock's current position.
        dragPlane = new Plane(
            mainCamera.transform.forward,
            transform.position
        );

        // Create another ray from the current mouse position.
        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        // Determine where the mouse ray intersects the drag plane.
        if (dragPlane.Raycast(ray, out float enter))
        {
            // Obtain the exact intersection point.
            Vector3 hitPoint = ray.GetPoint(enter);

            // Store the offset between the object's center
            // and the clicked position.
            //
            // This prevents snapping and keeps dragging natural.
            dragOffset = transform.position - hitPoint;
        }
    }

    // Updates the rock's position while dragging.
    private void Drag()
    {
        // Generate a ray from the current mouse position.
        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        // Determine whether the ray intersects the drag plane.
        if (dragPlane.Raycast(ray, out float enter))
        {
            // Compute the new position.
            //
            // ray.GetPoint() gives the mouse position projected
            // onto the drag plane.
            //
            // The stored offset is then added so the object
            // maintains its original grab position.
            Vector3 targetPosition = ray.GetPoint(enter) + dragOffset;

            // Keep the original Z coordinate so the object
            // cannot move toward or away from the camera.
            targetPosition.z = fixedZPosition;

            // Move the object directly to the calculated position.
            transform.position = targetPosition;
        }
    }

    // Called once when dragging ends.
    //
    // Restores the Rigidbody so Unity's physics engine
    // takes control again.
    private void EndDrag()
    {
        // Prevent duplicate execution.
        if (!isDragging)
            return;

        // Dragging has finished.
        isDragging = false;

        // Re-enable physics simulation.
        rb.isKinematic = false;

        // Restore gravity.
        rb.useGravity = true;

        // Force the Rigidbody to wake up immediately.
        //
        // This guarantees the physics engine begins simulating
        // the object without waiting.
        rb.WakeUp();

        // Reset movement to prevent any sudden velocity
        // from being carried over after dragging.
        rb.linearVelocity = Vector3.zero;

        // Reset rotational velocity.
        rb.angularVelocity = Vector3.zero;
    }
}