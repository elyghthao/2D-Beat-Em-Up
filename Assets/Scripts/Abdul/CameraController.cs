using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*/

public class CameraController : MonoBehaviour {
    // ============================================ PUBLIC VARIABLES ============================================
    // ---------------------- ROOM VARIABLES ----------------------
    [Header("Room Variables")]
    public List<room> rooms;    // Holds the structs of the room
    public room mainRoom;       // The main room. Considered the first room (if using multiple rooms) or the only room (if not using multiple rooms per level)

    // Struct for the rooms
    [System.Serializable]
    public struct room {
        public Vector3 boundsMin;       // The minimum x, y values defining the bounds of the room
        public Vector3 boundsMax;       // The maximum x, y values defining the bounds of the room
        public Vector3 eulerAngles;     // Rotation of the camera for this scene
        public Vector3 positionOffset;  // Offsets for camera position for x and y. Use this in conjuction with the angles to get a that perspective perfected.
        public float camSizeOffset;     // Camera size offset. Probably want to keep this all the same for all rooms and adjust other values.
        public float dampTime;          // Time it takes for the camera to reach a position (longer = slower) Probably want to keep this all the same for all rooms and adjust other values.
    }


    // ---------------------- MOUSE VARIABLES ----------------------
    // These do not effect the mouse but rather these are the settings for the camera to follow the mouse
    [Header("Mouse-Camera Variables")]
    public float mouseFactor = .1f;     // How much should the camera follow the mouse (a greater value means more)
    public float mouseLimit = 2;        // Limit the camera wont pass for going towards mouse (a greater value means there can be more offsets)


    // ---------------------- GENERAL VARIABLES ----------------------
    [Header("General Variables")]
    public GameObject player;


    // ============================================ PRIVATE VARIABLES ============================================
    private Transform plrTrans; 
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private float baseSize; // The main/base size of the camera.
    private room currentRoom;

    // Start is called before the first frame update
    void Start() {
        // Setting variables
        plrTrans = player.GetComponent<Transform>();
        cam = gameObject.GetComponent<Camera>();
        baseSize = cam.orthographicSize;
        currentRoom = mainRoom;

        // Mainly using this method for testing
    }

    // Update is called once per frame
    void Update() {
        // Player and Mouse Position Info
        currentRoom = getCurrentRoom();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xPlayerPos = plrTrans.position.x;
        float yPlayerPos = plrTrans.position.y;
        float zPlayerPos = plrTrans.position.z;
        float xDist = mousePos.x - xPlayerPos;
        float yDist = mousePos.y - yPlayerPos;

        // Setting up a new camera position
        float xCamPos = Mathf.Clamp(xPlayerPos, currentRoom.boundsMin.x, currentRoom.boundsMax.x) + Mathf.Clamp(xDist * mouseFactor, mouseLimit * -1, mouseLimit);
        float yCamPos = Mathf.Clamp(yPlayerPos, currentRoom.boundsMin.y, currentRoom.boundsMax.y) + Mathf.Clamp(yDist * mouseFactor, mouseLimit * -1, mouseLimit);
        float zCamPos = Mathf.Clamp(zPlayerPos, currentRoom.boundsMin.z, currentRoom.boundsMax.z);
        xCamPos += currentRoom.positionOffset.x;
        yCamPos += currentRoom.positionOffset.y;
        zCamPos += currentRoom.positionOffset.z;

        // Setting Camera Values
        Vector3 newCamPos = new Vector3(xCamPos, yCamPos, zCamPos);
        cam.orthographicSize = baseSize + currentRoom.camSizeOffset;
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, newCamPos, ref velocity, currentRoom.dampTime);

        Vector3 newPos = transform.position;
        transform.position = newPos;
        transform.eulerAngles = currentRoom.eulerAngles;
    }

    // Returns the closest room to the player
    private room getCurrentRoom() {
        room closestRoom = mainRoom;
        for (int i = 0; i < rooms.Count; i++) {
            if (getRoomDistance(mainRoom) < getRoomDistance(rooms[i])) {
                closestRoom = rooms[i];
            }
        }

        return closestRoom;
    }

    // Returns the distance of the given room to the player
    private float getRoomDistance(room distRoom) {
        Vector2 center = new Vector2();
        center.x = (distRoom.boundsMax.x + distRoom.boundsMin.x) / 2;
        center.y = (distRoom.boundsMax.y + distRoom.boundsMin.y) / 2;
        Vector2 playerCenter = new Vector2(player.transform.position.x, player.transform.position.y);
        float dist = Mathf.Sqrt(
            Mathf.Pow(center.x - center.x, 2) + Mathf.Pow(center.y - center.y, 2)
        );
        return dist;
    }
}
