using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

    bool shouldShake = false;
    static float duration = 5.0f;
    static float magnitude = 5.0f;

    private void Update() {
        if (this.shouldShake) {
            //StartCoroutine(Shake(CameraShaker.duration, CameraShaker.magnitude));
        }
    }

    public IEnumerator Shake (float duration, float magnitude) {
        Vector3 origPos = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration) {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;

            transform.localPosition = new Vector3(x, y, origPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = origPos;
        this.shouldShake = false;
    }

    // setter
    public void ShouldShake(bool shouldShake) {
        this.shouldShake = shouldShake;
    }

}
