using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    float cameraMinSize = 3f;
    [SerializeField]
    float cameraMaxSize = 25f;

    Vector2 lastFramePosition;
    Vector2 currFramePosition;

    public static InputManager Instance { get; protected set; }

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be more than 1 InputManager.");
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ManageCameraControl();
        ManageInput();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Tile GetTileUnderMouse()
    {
        return WorldManager.Instance.GetTileAtWorldCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void ManageInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            JobManager.Instance.CreateJobAt(GetTileUnderMouse());
        }
    }

    #region CAMERA INPUT
    void ManageCameraControl()
    {
        ManageCameraZoom();

        ManageCameraMovement();
    }

    void ManageCameraZoom()
    {
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, cameraMinSize, cameraMaxSize);
    }

    void ManageCameraMovement()
    {
        if (Input.GetButton("Fire3"))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }
    }
    #endregion
}
