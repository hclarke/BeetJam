using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameGUI : MonoBehaviour {

    public Texture[] healthIcons;
    public Rect healthRect;

    public GUIKillCount[] killCounters;
    public GUIStyle killCounterStyle;

    public int health;
    int killCount;

    void OnGUI() {
        if (Event.current.type != EventType.repaint) return;
        //draw health
        var healthTex = healthIcons[Mathf.Min(healthIcons.Length-1, health)];
        GUI.DrawTexture(healthRect, healthTex, ScaleMode.ScaleToFit, true);

        foreach (var counter in killCounters) {
            var content = new GUIContent(killCount.ToString(), counter.icon);
            killCounterStyle.Draw(counter.position, content, false, false, false, false);
        }
    }
}

[System.Serializable]
public class GUIKillCount {
    public string name;
    public Texture icon;
    public Rect position;
}
