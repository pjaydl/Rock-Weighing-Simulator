using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DraggableRock : MonoBehaviour
{
    //Temporarily commented out to avoid errors in the code
    //[SerializeField] private float followSpeed = 20f;

    private Rigidbody rb;
    private Camera mainCamera;

    private Plane dragPlane;
    private Vector3 dragOffset;

    private bool isDragging;

    private float fixedZPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }


    private void Update()
    {
        if (Mouse.current == null)
            return;


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (TryGetRockUnderMouse(out DraggableRock hitRock) && hitRock == this)
            {
                StartDrag();
            }
        }


        if (isDragging && !Mouse.current.leftButton.isPressed)
        {
            EndDrag();
        }
    }


    private void FixedUpdate()
    {
        if (isDragging)
        {
            Drag();
        }
    }


    private bool TryGetRockUnderMouse(out DraggableRock hitRock)
    {
        hitRock = null;

        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {  
            hitRock = hit.collider.GetComponentInParent<DraggableRock>();
        }

        return hitRock != null;
    }


    private void StartDrag()
    {
        isDragging = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;


        fixedZPosition = transform.position.z;


        dragPlane = new Plane(
            mainCamera.transform.forward,
            transform.position
        );


        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );


        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            dragOffset = transform.position - hitPoint;
        }
    }


    private void Drag()
    {
        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPosition = ray.GetPoint(enter) + dragOffset;

            // Lock depth movement
            targetPosition.z = fixedZPosition;

            transform.position = targetPosition;
    }
    }


    private void EndDrag()
    {
        if (!isDragging)
            return;


        isDragging = false;


        rb.isKinematic = false;
        rb.useGravity = true;


        rb.WakeUp();


        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}