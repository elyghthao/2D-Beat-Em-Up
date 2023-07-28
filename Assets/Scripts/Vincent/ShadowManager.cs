using UnityEngine;

public class ShadowManager : MonoBehaviour {
   private GameObject host;

   private void Awake() {
      host = transform.parent.gameObject;
   }

   private void Update() {
      transform.position = GetPointBelow();
   }

   public Vector3 GetPointBelow() {
      RaycastHit hit;
      var layer = 10;
      var layerMask = 1 << layer;
      var curPos = host.transform.position;
      //Debug.DrawRay(new Vector3(curPos.x, curPos.y + host.transform.localScale.y * 0.75f, curPos.z), Vector3.down * 10f, Color.red);
      if (Physics.Raycast(new Vector3(curPos.x, curPos.y + host.transform.localScale.y * 0.75f, curPos.z), Vector3.down,
             out hit, 10f, layerMask))
         if (hit.collider.CompareTag("Ground"))
            return new Vector3(transform.position.x, hit.point.y + 0.01f, transform.position.z);
      return new Vector3(curPos.x, curPos.y + 0.1f, curPos.z);
   }
}