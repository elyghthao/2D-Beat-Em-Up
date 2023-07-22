using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoundsManager : MonoBehaviour {
    private BoxCollider col;
    private Material mat;
    private void Awake() {
        col = GetComponent<BoxCollider>();
        mat = GetComponent<Renderer>().material;
        Debug.Log(mat);
    }

    public void setMatColor(Color color) {
        mat.color = color;
    }

    public Color getMatColor() {
        return mat.color;
    }

    public void setColliderActive(bool active) {
        if (col == null) {
            Debug.LogWarning("Collider was null, check to make sure you're calling AttackBoundsManager correctly");
            col = GetComponent<BoxCollider>();
        } else {
            col.enabled = active;
        }
    }
}
