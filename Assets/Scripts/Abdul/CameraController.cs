using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // Public Variables
    public GameObject player;
    public float dampTime = .23f; // Time it takes for the camera to reach a position (longer = slower)
    public float mouseFactor = .1f; // How much should the camera follow the mouse
    public float mouseLimit = 2; // Limit the camera wont pass for going towards mouse
    public float sizeOffset = 0; // Offets for the size/FOV of the camera
    public Vector2 minLimits; // The lower bound values for x and y the camera will never pass
    public Vector2 maxLimits; // The upper bound values for x and y the camera will never pass
    public Vector2 offsets; // Offsets for the camera

    // Private Variables
    private Transform plrTrans; 
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private List<Oscillation> sizeOsc = new List<Oscillation>(); // Size Oscillation
    private List<Oscillation> xPosOsc = new List<Oscillation>(); // x Position Oscillation
    private List<Oscillation> yPosOsc = new List<Oscillation>(); // y Position Oscillation
    private float baseSize; // The main/base size of the camera.

    // Start is called before the first frame update
    void Start() {
        // Setting variables
        plrTrans = player.GetComponent<Transform>();
        cam = gameObject.GetComponent<Camera>();
        baseSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        // Player Position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xPlayerPos = plrTrans.position.x;
        float yPlayerPos = plrTrans.position.y;
        float xDist = mousePos.x - xPlayerPos;
        float yDist = mousePos.y - yPlayerPos;

        // Setting up a new camera position
        float xCamPos = Mathf.Clamp(xPlayerPos, minLimits.x, maxLimits.x);
        float yCamPos = Mathf.Clamp(yPlayerPos, minLimits.y, maxLimits.y);
        xCamPos += Mathf.Clamp(xDist * mouseFactor, mouseLimit * -1, mouseLimit);
        yCamPos += Mathf.Clamp(yDist * mouseFactor, mouseLimit * -1, mouseLimit);;
        
        // Updating Size Oscillations
        float newSize = baseSize;
        for (int i = 0; i < sizeOsc.Count; i++) {
            if (sizeOsc[i] == null) {
                sizeOsc.RemoveAt(i);
            } else {
                newSize += sizeOsc[i].getValue();
            }
        }

        // Updating Pos Oscillations
        for (int i = 0; i < xPosOsc.Count; i++) {
            if (xPosOsc[i] == null) {
                xPosOsc.RemoveAt(i);
            } else {
                xCamPos += xPosOsc[i].getValue();
            }
        }
        for (int i = 0; i < yPosOsc.Count; i++) {
            if (yPosOsc[i] == null) {
                yPosOsc.RemoveAt(i);
            } else {
                yCamPos += yPosOsc[i].getValue();
            }
        }

        // Offsets
        xCamPos += offsets.x;
        yCamPos += offsets.y;
        newSize += sizeOffset;

        // Setting Camera Values
        Vector3 newCamPos = new Vector3(xCamPos, yCamPos, -10f);
        cam.orthographicSize = newSize;
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, newCamPos, ref velocity, dampTime);
    }

    // Add an oscillation effect to the size of the camera
    public void addSizeOscillation(Oscillation newOscillation) { sizeOsc.Add(newOscillation); }

    // Add an oscillation effect to the position of the camera
    public void addPosOscillation(Oscillation xOscillation, Oscillation yOscillation) { 
        xPosOsc.Add(xOscillation);
        yPosOsc.Add(yOscillation);
    }

    // Sets the basesize
    public void setCameraSize(float size) { baseSize = size; }
}
