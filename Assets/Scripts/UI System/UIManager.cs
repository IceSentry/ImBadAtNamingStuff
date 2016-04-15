using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text tooltipUI;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        string tooltip = "";

        Tile tile = WorldManager.Instance.GetTileAtWorldCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (tile != null) {
            tooltip += tile.Type;
            //tooltip += " at " + tile.Position;
        }

        //tooltip += "\n" + Camera.main.ScreenToWorldPoint(Input.mousePosition);

        tooltipUI.text = tooltip;
    }
}
