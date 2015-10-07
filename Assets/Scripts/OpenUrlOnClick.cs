using UnityEngine;
using System.Collections;

public class OpenUrlOnClick : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}


    public void OpenUrl(string url) {
        Application.OpenURL(url);
    }
}
