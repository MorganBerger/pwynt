using Unity.VisualScripting;
using UnityEngine;
 
public class DraggableObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public bool isDragging = false;

    private DraggingManager dragManager;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        dragManager = DraggingManager.Instance;

        dragManager.dragPlaneDidChange.AddListener(plane => {
            // if (plane.)
        });
    }

    private void OnMouseUp() {
        _rigidbody.velocity = Vector3.zero;
        isDragging = false;
    }

    private void OnMouseDown() {
        isDragging = true;
    }

    
    private void LateUpdate() {

    }

    private void OnMouseDragWorldSpace() {
        var position = transform.position;

        if (!dragManager.currentDragPlane) 
            return;
        
        var dragPlane = dragManager.currentDragPlane;
        var mousePos = dragManager.currentMousePos;
        
        var dist = Vector3.Distance(mousePos, position);
        var speed = Mathf.Exp(8 * dist) + 0.5f;
        var cardSpeed = Mathf.Clamp(speed, 0f, 20f);

        var difference = mousePos - position;
        var worldVelocity = difference * cardSpeed;

        var localVelocity = transform.InverseTransformDirection(worldVelocity);
        localVelocity.Scale(dragPlane.dragAxis);

        _rigidbody.velocity = transform.TransformDirection(localVelocity);

        if (dragPlane.cardRotate) {
            var rotationSpeed = difference * 50;
            _rigidbody.rotation = Quaternion.Euler(new Vector3(rotationSpeed.z, _rigidbody.rotation.y, -rotationSpeed.x));

        } else if (transform.rotation != dragPlane.transform.rotation) {
            transform.rotation = dragPlane.transform.rotation;
        }
    }

    private void OnMouseDrag() {
        OnMouseDragWorldSpace();
    }
}