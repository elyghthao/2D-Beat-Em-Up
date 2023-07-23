using UnityEngine;

public class AttackBoundsManager : MonoBehaviour {
    private BoxCollider _collider;
    private Material _material;
    private void Awake() {
        _collider = GetComponent<BoxCollider>();
        _material = GetComponent<Renderer>().material;
    }

    public void SetMatColor(Color color) {
        _material.color = color;
    }

    public Color GetMatColor() {
        return _material.color;
    }

    public void SetColliderActive(bool active) {
        if (_collider == null) {
            Debug.LogWarning("Collider was null, check to make sure you're calling AttackBoundsManager correctly");
            _collider = GetComponent<BoxCollider>();
        } else {
            _collider.enabled = active;
        }
    }
}
