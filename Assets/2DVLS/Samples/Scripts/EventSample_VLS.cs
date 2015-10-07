using UnityEngine;

public class EventSample_VLS: MonoBehaviour {
    private Color c = Color.black;
    public Material hitLightMaterial;
    public AudioClip hitSound;

    private int id;
    private bool isDetected;
    public AudioClip whiteSound;

    private void Start() {
        id = gameObject.GetInstanceID();

        Light2D.RegisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.RegisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.RegisterEventListener(LightEventListenerType.OnExit, OnLightExit);
    }

    private void OnDestroy() {
        /* (!) Make sure you unregister your events on destroy. If you do not
         * you might get strange errors (!) */

        Light2D.UnregisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.UnregisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.UnregisterEventListener(LightEventListenerType.OnExit, OnLightExit);
    }

    private void Update() {
        if (isDetected)
            GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, c,
                Time.deltaTime*10f);
        else
            GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.black,
                Time.deltaTime*5f);

        isDetected = false;
    }

    private void OnLightEnter(Light2D l, GameObject g) {
        if (g.GetInstanceID() == id){
            c += l.LightColor;
            AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.1f);
        }
    }

    private void OnLightStay(Light2D l, GameObject g) {
        if (g.GetInstanceID() == id){
            isDetected = true;
        }
    }

    private void OnLightExit(Light2D l, GameObject g) {
        if (g.GetInstanceID() == id){
            c -= l.LightColor;

            if ((GetComponent<Renderer>().material.color.r > 0.95f) &&
                (GetComponent<Renderer>().material.color.g > 0.95f) &&
                (GetComponent<Renderer>().material.color.b > 0.95f)){
                AudioSource.PlayClipAtPoint(whiteSound, transform.position, 0.5f);
                var l2d = Light2D.Create(transform.position, hitLightMaterial, new Color(.8f, .8f, 0.6f),
                    Random.Range(3, 5f));
                l2d.ShadowLayer = 0;
                l2d.transform.Rotate(0, 0, Random.Range(10, 80f));
                Destroy(l2d.gameObject, 0.2f);
            }
        }
    }
}