using UnityEngine;

namespace Assets.Scripts {
    public class WallDestroyer: MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Wall"){
                Destroy(other.gameObject);
            }
        }
    }
}