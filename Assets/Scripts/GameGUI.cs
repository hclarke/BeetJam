using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameGUI : MonoBehaviour {

    public Texture[] healthIcons;
    public Rect healthRect;

    public GUIKillCount[] killCounters;
    public int health;
    public int killCount;

    void OnGUI() {
        var style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        if (Event.current.type != EventType.repaint) return;
        //draw health
        var healthTex = healthIcons[Mathf.Clamp(health, 0, healthIcons.Length-1)];
        GUI.DrawTexture(healthRect, healthTex, ScaleMode.ScaleToFit, true);

        foreach (var counter in killCounters) {
            var content = new GUIContent(killCount.ToString(), counter.icon);
            GUI.DrawTexture(counter.position, counter.icon, ScaleMode.ScaleToFit, true);
            style.Draw(counter.textPos, killCount.ToString(), false, false, false, false);
        }
    }
}

[System.Serializable]
public class GUIKillCount {
    public string name;
    public Texture icon;
    public Rect position;
    public Rect textPos;
}
