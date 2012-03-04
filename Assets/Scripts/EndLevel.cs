using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {
    public float delay = 1f;
    public float fadeTime = 2f;
    public float delay2 = 1f;
    public Texture tex;

    public string level;

    float fadeAmount = 0;
	IEnumerator Start () {
        yield return new WaitForSeconds(delay);
        var t = 0f;
        while (t < fadeTime) {
            yield return null;
            t += Time.deltaTime;
            var u = t / fadeTime;
            Fade(u);
        }
        Fade(1);
        yield return new WaitForSeconds(delay2);
        Application.LoadLevel(level);
	}

    void Fade(float t) {
        fadeAmount = t;
    }

    void OnGUI() {
        GUI.color = new Color(1, 1, 1, fadeAmount);
        GUI.depth = -10;
        var r = new Rect(0, 0, Screen.width, Screen.height);
        GUI.DrawTexture(r, tex, ScaleMode.StretchToFill);
    }
}
