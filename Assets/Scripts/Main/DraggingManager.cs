using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

class DraggingManager: MonoBehaviour {

    public static DraggingManager Instance { get; private set; }

    public Vector3 currentMousePos;
    public DragPlane currentPlane;

    private List<DraggableObject> _draggables;

    public UnityEvent<DraggableObject, DragPlane> didReleaseObject;
    public UnityEvent<DraggableObject, DragPlane, DragPlane> didDragObject;

    private void Awake() {
        print("DraggingManager: Awake()");
        Instance = this;
    }

    private void Start() {
        Setup();
    }

    private void Setup() {
        _draggables = new List<DraggableObject>();

        DraggableObject[] draggables = GetComponentsInChildren<DraggableObject>();
        print("draggables count: " + draggables.Length);

        _draggables.AddRange(draggables);
        foreach (var d in _draggables) {
            d.onDrag.AddListener(OnDragObject);
            d.onRelease.AddListener(OnReleaseObject);
        }
    }

    private DraggableObject _currentDraggedObject = null;
    private void OnDragObject(DraggableObject obj) {
        _currentDraggedObject = obj;
    }

    private void OnReleaseObject(DraggableObject obj) {
        if (obj == _currentDraggedObject) {
            didReleaseObject.Invoke(obj, currentPlane);
            _currentDraggedObject = null;
        } else {
            print("MIGHT BE A PROBLEM THERE. DROPPED OBJECT IS NOT DRAGGED OBJECT...");
        }
    }

    private void OnDestroy() {
        foreach (var d in _draggables) {
            d.onDrag.RemoveListener(OnDragObject);
            d.onRelease.RemoveListener(OnReleaseObject);
        }
    }

    [SerializeField]
    public RaycastHit[] currenthits;
    private void LateUpdate() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        if (hits.Length == 0) {
            return;
        }
        currenthits = hits;

        DragPlane targetPlane = null;
        RaycastHit hit = new RaycastHit();
        
        var rayOrigin = ray.origin;
        double distance = double.MaxValue;
        
        foreach (var h in hits) {
            var plane = h.transform.GetComponent<DragPlane>();
            if (plane == null) continue;

            var dist = Vector3.Distance(rayOrigin, h.point);
            if (dist < distance) {
                distance = dist;
                targetPlane = plane;
                hit = h;
            }
        }
        
        if (targetPlane == null)
            return;
        
        var oldPlane = currentPlane;
        currentPlane = targetPlane;

        if (oldPlane == null)
            oldPlane = currentPlane;

        if (oldPlane != targetPlane && _currentDraggedObject != null) {
            didDragObject.Invoke(_currentDraggedObject, oldPlane, currentPlane);
        }

        currentMousePos = new Vector3(
            hit.point.x + targetPlane.padding.x,
            hit.point.y + targetPlane.padding.y,
            hit.point.z + targetPlane.padding.z
        );

        if (currentPlane.drawsLine) {
            Debug.DrawLine(ray.origin, hit.point, currentPlane.lineColor);
        }
    }
}