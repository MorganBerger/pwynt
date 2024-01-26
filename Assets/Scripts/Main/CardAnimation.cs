using System.Collections;
using UnityEngine;

struct CardAnimation {
    public static IEnumerator MoveTo(Transform objectToMove, Vector3 b, float dur) {
        // Debug.Log("move");
        float t = 0f;
        Vector3 start = objectToMove.localPosition;
        
        while (t < dur) {
            objectToMove.localPosition = Vector3.Lerp(start, b, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        objectToMove.localPosition = b;
    }

    public static IEnumerator RotateTo(Transform target, Quaternion rot, float dur) {
        // Debug.Log("rotate");
        float t = 0f;
        Quaternion start = target.localRotation;
        while(t < dur) {
            target.localRotation = Quaternion.Slerp(start, rot, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        target.localRotation = rot;
    }
}