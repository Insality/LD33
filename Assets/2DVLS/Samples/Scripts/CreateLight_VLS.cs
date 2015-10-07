using System.Collections.Generic;
using UnityEngine;

public class CreateLight_VLS: MonoBehaviour {
    private readonly List<Light2D> lightsInScene = new List<Light2D>();
    private Vector2[] circleLookup;
    public float initialRadius = 25f;
    public bool isPro = false;
    private Material lMat;
    public float maxRadius = 25;
    public float minRadius = 1;
    private int points = 5;
    private Light2D selectedLight;
    private int selection;
    public Vector3 spawnPoint = new Vector3(0, 3, 0);

    private bool wasHit;
    private Rect windowRect = new Rect(20, 20, 300, 380);

    private void Start() {
        circleLookup = new Vector2[points];
        for (var i = 0; i < circleLookup.Length; i++){
            var rad = (i*(360f/points))*Mathf.Deg2Rad;
            circleLookup[i] = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
        }

        Random.seed = gameObject.GetInstanceID();
        CreateLight(spawnPoint);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)){
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
        }

        if (Input.GetMouseButtonDown(0)){
            foreach (var l in lightsInScene){
                var box = new Rect(l.transform.position.x - 1, l.transform.position.y - 1, 2, 2);
                wasHit = box.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                if (wasHit){
                    selectedLight = l;
                    break;
                }
            }
        }

        if (Input.GetMouseButtonDown(1)){
            CreateLight(Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10));
        }

        if (wasHit && Input.GetMouseButton(0)){
            selectedLight.transform.position = Vector3.Lerp(selectedLight.transform.position,
                Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), Time.deltaTime*20f);
                // new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);
        }
    }

    private void OnPostRender() {
        if (!lMat)
            lMat = new Material(Shader.Find("VertexLit"));

        GL.PushMatrix();
        lMat.SetPass(0);
        //GL.LoadOrtho();

        foreach (var l in lightsInScene){
            //Rect box = new Rect(l.transform.position.x - 1, l.transform.position.y - 1, 2, 2);

            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            for (var i = 1; i < circleLookup.Length; i++){
                GL.Vertex(l.transform.position + (Vector3) circleLookup[i - 1]);
                GL.Vertex(l.transform.position + (Vector3) circleLookup[i]);
            }

            GL.Vertex(l.transform.position + (Vector3) circleLookup[circleLookup.Length - 1]);
            GL.Vertex(l.transform.position + (Vector3) circleLookup[0]);

            GL.End();
        }
        GL.PopMatrix();
    }

    private void CreateLight(Vector3 position) {
        selectedLight = Light2D.Create(position, new Color(1f, 0.5f, 0f, 0f), initialRadius);
        selectedLight.EnableEvents = true;

        if (isPro)
            selectedLight.gameObject.layer = 31;

        lightsInScene.Add(selectedLight);
    }

    private void OnGUI() {
        windowRect = GUI.Window(0, windowRect, WindowFunc, "Settings");
    }

    private void WindowFunc(int id) {
        if (GUILayout.Button("Clear Lights")){
            for (var i = 0; i < lightsInScene.Count; i++){
                Destroy(lightsInScene[i].gameObject);
            }

            lightsInScene.Clear();
            lightsInScene.TrimExcess();

            CreateLight(Vector3.zero);
        }
        if (GUILayout.Button("Camera Color")){
            selection = -1;
        }

        GUILayout.Space(10);
        selection = GUILayout.SelectionGrid(selection,
            new[] {
                "Light Radius", "Light Color", "Light Cone Angle", "Light Cone Start", "Toggle Enabled",
                "Toggle Fullscreen"
            }, 1);

        GUILayout.Space(30);
        switch (selection){
            case -1:
                Camera.main.backgroundColor = AdjustColor(Camera.main.backgroundColor);
                break;
            case 0:
                if (selectedLight != null){
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Radius", GUILayout.Width(80));
                        selectedLight.LightRadius = GUILayout.HorizontalSlider(selectedLight.LightRadius, minRadius,
                            maxRadius);
                    }
                    GUILayout.EndHorizontal();
                }
                break;
            case 1:
                if (selectedLight != null)
                    selectedLight.LightColor = AdjustColor(selectedLight.LightColor);
                break;
            case 2:
                if (selectedLight != null){
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Cone Angle", GUILayout.Width(80));
                        selectedLight.LightConeAngle = GUILayout.HorizontalSlider(selectedLight.LightConeAngle, 360, 0);
                    }
                    GUILayout.EndHorizontal();
                }
                break;
            case 3:
                if (selectedLight != null){
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Cone Start", GUILayout.Width(80));
                        selectedLight.LightConeStart = GUILayout.HorizontalSlider(selectedLight.LightConeStart, 0, 360);
                    }
                    GUILayout.EndHorizontal();
                }
                break;
            case 4:
                if (selectedLight != null){
                    selectedLight.LightEnabled = !selectedLight.LightEnabled;
                    selection = 0;
                }
                break;
            case 5:
                selection = 0;
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
                break;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    private Color AdjustColor(Color color) {
        var c = color;

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Red", GUILayout.Width(80));
            c.r = GUILayout.HorizontalSlider(c.r, 0f, 1f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Green", GUILayout.Width(80));
            c.g = GUILayout.HorizontalSlider(c.g, 0f, 1f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Blue", GUILayout.Width(80));
            c.b = GUILayout.HorizontalSlider(c.b, 0f, 1f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Alpha", GUILayout.Width(80));
            c.a = GUILayout.HorizontalSlider(c.a, 0f, 1f);
        }
        GUILayout.EndHorizontal();

        return c;
    }
}