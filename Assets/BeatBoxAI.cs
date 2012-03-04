using UnityEngine;
using System.Collections;

public class BeatBoxAI : MonoBehaviour {
	public float BURROWED_HEIGHT = -1;
	public float UNBURROWED_HEIGHT = 0;
	public float RAM_SPEED = 1.0f;
    public float RAM_TIME = 2.0f;
	private float unburrowedTime = 0.0f;

    bool inRange = false;

	// Update is called once per frame
	void Update () {
        if (!FaceMoveControl.instance) return;
        var pos = FaceMoveControl.instance.transform.position;
        bool close = Vector3.Distance(pos, transform.position) < RAM_SPEED * RAM_TIME * 0.7f;
        if (!inRange && close) {
            inRange = true;
            Ram(FaceMoveControl.instance.gameObject);
        }
        else if (!close) {
            inRange = false;
        }
        pos = transform.position;
		if (unburrowedTime <= 0) {
			pos = new Vector3(transform.position.x, BURROWED_HEIGHT, transform.position.z);
		} else {
			pos += transform.forward * RAM_SPEED * Time.deltaTime;
			unburrowedTime -= Time.deltaTime;
		}

        var posx = Mathf.RoundToInt(pos.x - 0.5f);
        var posy = Mathf.RoundToInt(pos.z - 0.5f);
        if (TileRenderer.Height(posx, posy) > 0.01f) {
            transform.position = pos;
        }
	}
	

    void Ram(GameObject other) {
        //unburrow
        unburrowedTime = RAM_TIME;
        transform.position = new Vector3(transform.position.x, UNBURROWED_HEIGHT, transform.position.z);

        //face with random offset
        var r = transform.rotation;
        r.SetLookRotation(other.transform.position - this.transform.position);
        r = Quaternion.FromToRotation(new Vector3(1, 0, 0), new Vector3(1, 0, Random.Range(-0.5f, 0.5f))) * r;
        transform.rotation = r;
    }
}
