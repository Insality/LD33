using UnityEngine;

public class RandomAimRotate_VLS: MonoBehaviour {
    private Light2D l;
    private int position;
    private Vector3 tPos = Vector3.zero;

    private void Start() {
        l = gameObject.GetComponent<Light2D>();
        InvokeRepeating("ChangePosition", 0, 1);
    }

    private void Update() {
        tPos = Vector3.MoveTowards(tPos, new Vector3(0, position, 0), Time.deltaTime*25f);
        l.LookAt(tPos);
    }

    private void ChangePosition() {
        position = Random.Range(-4, 5)*2;
    }
}