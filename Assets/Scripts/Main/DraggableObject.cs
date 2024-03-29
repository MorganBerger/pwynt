using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DraggableObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public bool isDragging = false;

    public UnityEvent<DraggableObject> onDrag;
    public UnityEvent<DraggableObject> onRelease;

    private float dragSpeed = 5f;

    public bool draggingEnabled = true;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnMouseUp() {
        _rigidbody.velocity = Vector3.zero;
        isDragging = false;
        onRelease.Invoke(this);
    }

    private void OnMouseDown() {
        if (!draggingEnabled)
            return;

        isDragging = true;
        onDrag.Invoke(this);
    }

    private void OnMouseDragWorldSpace() {

        var position = transform.position;

        var dragPlane = DraggingManager.Instance.currentPlane;
        var mousePos = DraggingManager.Instance.currentMousePos;
        
        // var dist = Vector3.Distance(mousePos, position);
        // var speed = Mathf.Exp(8 * dist) + 0.5f;
        // var cardSpeed = Mathf.Clamp(speed, 0f, 20f);
        var cardSpeed = dragSpeed;

        var difference = mousePos - position;

        // No plane drag gesture
        if (dragPlane == null) {
            difference = Vector3.zero;
        }

        var worldVelocity = difference * cardSpeed;
        var localVelocity = transform.InverseTransformDirection(worldVelocity);

        // No plane drag gesture
        if (dragPlane != null) {
            localVelocity.Scale(dragPlane.dragAxis);
        }

        _rigidbody.velocity = transform.TransformDirection(localVelocity);

        if (dragPlane == null || dragPlane.cardRotate) {
            var rotationSpeed = difference * 50;
            _rigidbody.rotation = Quaternion.Euler(new Vector3(rotationSpeed.z, _rigidbody.rotation.y, -rotationSpeed.x));
        } else if (transform.rotation != dragPlane.transform.rotation) {
            transform.rotation = dragPlane.transform.rotation;
        }
    }

    public bool canDrop = false;
    public void Drop() {
        var collider = GetComponent<BoxCollider>();
        collider.center = new Vector3(collider.center.x, collider.center.y, 0);
        collider.size = new Vector3(collider.size.x, 0.005f, 0.183f);
        collider.isTrigger = false;

        _rigidbody.useGravity = true;
    }

    private void OnMouseDrag() {
        if (!draggingEnabled)
            return;

        OnMouseDragWorldSpace();
    }
}