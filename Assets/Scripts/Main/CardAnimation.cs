using System.Collections;
using UnityEngine;

struct CardAnimation {
    public static IEnumerator MoveTo(CardBehaviour target, Vector3 b, float dur) {
        Transform transform = target.transform;
        Vector3 start = transform.localPosition;
        float t = 0f;
        
        // target.animating = true;

        while (t < dur) {
            transform.localPosition = Vector3.Lerp(start, b, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        transform.localPosition = b;

        // target.animating = false;
    }

    public static IEnumerator RotateTo(CardBehaviour target, Quaternion rot, float dur) {
        Transform transform = target.transform;
        Quaternion start = transform.localRotation;
        float t = 0f;

        // target.animating = true;

        while(t < dur) {
            transform.localRotation = Quaternion.Slerp(start, rot, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        transform.localRotation = rot;

        // target.animating = false;
    }

    // public static IEnumerator MoveTo(Card target, Vector3 b, float dur) {
    //     Transform transform = target.transform;
    //     Vector3 start = transform.localPosition;
    //     float t = 0f;
        
    //     target.animating = true;

    //     while (t < dur) {
    //         transform.localPosition = Vector3.Lerp(start, b, t / dur);
    //         yield return null;
    //         t += Time.deltaTime;
    //     }
    //     transform.localPosition = b;

    //     target.animating = false;
    // }

    // public static IEnumerator RotateTo(Card target, Quaternion rot, float dur) {
    //     Transform transform = target.transform;
    //     Quaternion start = transform.localRotation;
    //     float t = 0f;

    //     target.animating = true;

    //     while(t < dur) {
    //         transform.localRotation = Quaternion.Slerp(start, rot, t / dur);
    //         yield return null;
    //         t += Time.deltaTime;
    //     }
    //     transform.localRotation = rot;

    //     target.animating = false;
    // }
}