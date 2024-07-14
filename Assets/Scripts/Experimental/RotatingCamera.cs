using System.Collections;
using UnityEngine;

public class RotatingCamera: MonoBehaviour {
    IEnumerator rotate;

    public Transform target;    // The target transform around which the camera will rotate
    public Transform focusPoint; // The transform the camera will look at

    public float rotationSpeed = 10f; // Speed of rotation

    private void Start() {
        rotate = RotateCamera();
        StartCoroutine(rotate); 
    }

    private IEnumerator RotateCamera() {
        while (true) {
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
            transform.LookAt(focusPoint);
            
            yield return null;
        }
    }
}