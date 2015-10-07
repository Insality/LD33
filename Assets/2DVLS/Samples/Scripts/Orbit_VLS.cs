using UnityEngine;

public class Orbit_VLS: MonoBehaviour {
    public Transform objectToOrbit = null;
    public float orbitSpeed = 25f;
    public Vector3 pointToOrbit = Vector3.zero;

    private void Update() {
        if (!objectToOrbit)
            transform.RotateAround(pointToOrbit, Vector3.forward, orbitSpeed*Time.deltaTime);
        else
            transform.RotateAround(objectToOrbit.position, Vector3.forward, orbitSpeed*Time.deltaTime);
    }
}