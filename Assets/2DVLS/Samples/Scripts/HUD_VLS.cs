using UnityEngine;

[ExecuteInEditMode]
public class HUD_VLS: MonoBehaviour {
    public Vector2 position = new Vector2(0, 0);

    private void OnGUI() {
        GUILayout.BeginArea(new Rect((Screen.width*position.x), (Screen.height*position.y), 200, 400));
            //position.x, position.y, 200, 400));
        {
            GUILayout.Label("Lights Rendered: \t\t" + Light2D.TotalLightsRendered);
            GUILayout.Label("Light Mesh Updates: \t" + Light2D.TotalLightsUpdated);
        }
        GUILayout.EndArea();
    }
}