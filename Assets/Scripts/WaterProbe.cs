using UnityEngine;
using System.Collections;

public class WaterProbe : MonoBehaviour {

    void OnDrawGizmos() {
        var water = TileRenderer.OnWaterTile(transform.position.x, transform.position.z);
        Gizmos.color = water ? Color.blue : Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
