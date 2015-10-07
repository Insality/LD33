using UnityEngine;

public class MovableWall: MonoBehaviour {
    public bool IsLeft;

    public float WallSpeed;
    public float WallkingWide;

    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        var MoveVector = new Vector3(WallSpeed*Time.deltaTime, 0, 0);
        if (IsLeft) MoveVector.x *= -1;

        transform.position += MoveVector;

        if (transform.position.x < -WallkingWide){
            IsLeft = false;
        }
        if (transform.position.x > WallkingWide){
            IsLeft = true;
        }
    }
}