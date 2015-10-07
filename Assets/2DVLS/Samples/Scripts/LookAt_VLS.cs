using UnityEngine;

[RequireComponent(typeof (Light2D))]
public class LookAt_VLS: MonoBehaviour {
    private Light2D lightRef;
    public float smoothLookSpeed = 5.0f;

    private Vector3 tPos = Vector3.zero;
    public Transform target = null;

    private void Start() {
        lightRef = gameObject.GetComponent<Light2D>();
    }

    private void Update() {
        tPos = Vector3.Lerp(tPos, target.position, Time.deltaTime*smoothLookSpeed);
        lightRef.LookAt(tPos);
    }
}