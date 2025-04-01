using System.Collections;
using UnityEngine;

struct CardAnimation {
    public static IEnumerator MoveTo(CardBehaviour target, Vector3 b, float dur) {
        Transform transform = target.transform;
        Vector3 start = transform.localPosition;
        float t = 0f;
        
        while (t < dur) {
            transform.localPosition = Vector3.Lerp(start, b, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        transform.localPosition = b;
     }

    public static IEnumerator RotateTo(CardBehaviour target, Quaternion rot, float dur) {
        Transform transform = target.transform;
        Quaternion start = transform.localRotation;
        float t = 0f;

        while(t < dur) {
            transform.localRotation = Quaternion.Slerp(start, rot, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        transform.localRotation = rot;
    }
}