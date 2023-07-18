using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour {
    private GameObject host;

    private void Awake() {
        host = transform.parent.gameObject;
    }

    void Update() {
        transform.position = GetPointBelow();
    }

    public Vector3 GetPointBelow() {
        RaycastHit hit;
        Vector3 curPos = host.transform.position;
        Debug.DrawRay(new Vector3(curPos.x, curPos.y + host.transform.localScale.y * 0.75f, curPos.z), Vector3.down * 10f, Color.red);
        if (Physics.Raycast(new Vector3(curPos.x, curPos.y + host.transform.localScale.y * 0.75f, curPos.z), Vector3.down, out hit, 10f)) {
            Debug.Log("Hit");
            if (hit.collider.CompareTag("Ground")) {
                return new Vector3(transform.position.x, hit.point.y + 0.01f, transform.position.z);
            }
        }
        Debug.Log("Not hitting");
        return new Vector3(curPos.x, curPos.y + 0.1f, curPos.z);
    }
}
