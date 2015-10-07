using UnityEngine;

public class SinMove_VLS: MonoBehaviour {
    public Vector3 axis = new Vector3(0, 0, 1);
    public float magnitude = 5;

    private Vector3 origPos = Vector3.zero;
    public float speed = 5;

    private void Start() {
        origPos = transform.position;
    }

    private void Update() {
        transform.position = origPos + (axis*Mathf.Sin(Time.time*speed)*magnitude);
    }
}