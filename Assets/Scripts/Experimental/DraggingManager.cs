using UnityEngine;
using UnityEngine.Events;

class DraggingManager: MonoBehaviour {

    public static DraggingManager Instance { get; private set; }

    public Vector3 currentMousePos;
    public DragPlane currentDragPlane;

    public UnityEvent<DragPlane> dragPlaneDidChange;

    private void Awake() {
        Instance = this;
    }

    private void LateUpdate() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        if (hits.Length == 0)
            return;

        var hit = hits[0];
        var dragPlane = hit.transform.GetComponent<DragPlane>();
        
        if (dragPlane == null) { 
            currentDragPlane = null;
            return;
        }

        if (dragPlane != currentDragPlane) {
            dragPlaneDidChange.Invoke(dragPlane);
        }

        currentDragPlane = dragPlane;
        if (dragPlane.drawsLine) {
            Debug.DrawLine(ray.origin, hit.point, dragPlane.lineColor);
        }

        currentMousePos = new Vector3(
            hit.point.x + dragPlane.padding.x,
            hit.point.y + dragPlane.padding.y,
            hit.point.z + dragPlane.padding.z
        );
    }
}