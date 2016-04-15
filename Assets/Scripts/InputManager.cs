using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    [SerializeField]
    private float cameraMinSize = 3f;

    [SerializeField]
    private float cameraMaxSize = 25f;

    private Vector2 lastFramePosition;
    private Vector2 currFramePosition;

    public static InputManager Instance { get; protected set; }

    // Use this for initialization
    private void Start() {
        if (Instance != null) {
            Debug.LogError("There should never be more than 1 InputManager.");
        }
        Instance = this;
    }

    // Update is called once per frame
    private void Update() {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ManageCameraControl();
        ManageMouseInput();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Tile GetTileUnderMouse() {
        return WorldManager.Instance.GetTileAtWorldCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void ManageMouseInput() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (Input.GetButtonDown("Fire1")) {
            if (GetTileUnderMouse() != null)
                JobManager.Instance.CreateJobAt(GetTileUnderMouse());
        }
    }

    #region CAMERA INPUT

    private void ManageCameraControl() {
        ManageCameraZoom();

        ManageCameraMovement();
    }

    private void ManageCameraZoom() {
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, cameraMinSize, cameraMaxSize);
    }

    private void ManageCameraMovement() {
        if (Input.GetButton("Fire3")) {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }
    }

    #endregion CAMERA INPUT
}