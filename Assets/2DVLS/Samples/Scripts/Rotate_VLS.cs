using UnityEngine;

public class Rotate_VLS: MonoBehaviour {
    public Vector3 Axis = new Vector3(0, 0, 1);
    public float speed = 100;

    private void FixedUpdate() {
        transform.Rotate(Axis*Time.deltaTime*speed);
    }
}