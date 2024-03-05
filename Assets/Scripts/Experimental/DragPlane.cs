using UnityEngine;

public class DragPlane: MonoBehaviour {

    public Vector3 padding = Vector3.zero;
    public Vector3 dragAxis = new Vector3(1, 1, 1);
    
    public bool cardRotate = false;

    public bool drawsLine = true;
    public Color lineColor = Color.red;

    public bool Is(MonoBehaviour go) {
        return this == go.GetComponent<DragPlane>();
    }

}