using UnityEngine;

public class CameraManager : MonoBehaviour {

    // Use this for initialization
    void Start() {
        transform.position = new Vector3(WorldManager.Instance.Width / 2, WorldManager.Instance.Height / 2, transform.position.z);
    }

}
