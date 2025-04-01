using UnityEngine;

// public class CustomCollider: Collider {
//     public string _ID;
// }

[ExecuteInEditMode]
public class CardDebugUI: MonoBehaviour {

    void Awake() {
        // var array = GetComponents<Collider>();
    }

    [ExecuteInEditMode]
    void Start() {
        Vector3 pos = transform.position + Camera.main.transform.rotation * Vector3.forward;
        transform.LookAt(pos, Camera.main.transform.rotation * Vector3.up);
        // transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.y, 0);
    }

    void Update() {

    }

}