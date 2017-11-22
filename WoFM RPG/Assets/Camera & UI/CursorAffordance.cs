using UnityEngine;

[RequireComponent(typeof(CameraRayCaster))]
public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursor = null;
    [SerializeField] Texture2D strikeCursor = null;
    [SerializeField] Texture2D notAllowedCursor = null;
    CameraRayCaster cameraRayCaster;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0,0);
	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRayCaster>();
        cameraRayCaster.LayerChangeObservers += OnLayerHit; // register delegate

    }
	public void OnLayerHit(Layer layer)
    {

        switch (layer)
        {
            case Layer.RaycastEndStop:
            case Layer.Obstacle:
                Cursor.SetCursor(notAllowedCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.IOs:
                Cursor.SetCursor(strikeCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
                break;
        }
    }
    // TODO consider de-registering LayerChangeObservers delegate on leaving all game scenes
	// Update is called once per frame
	void Update () {
    }
}
