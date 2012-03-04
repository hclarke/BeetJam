using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GameGUI : MonoBehaviour {

    public Texture[] healthIcons;
    public Rect healthRect;

    public GUIKillCount[] killCounters;
    public int health;

    public static GameGUI instance;

    Dictionary<string, int> killCounts = new Dictionary<string, int>();

    void Awake() {
        instance = this;
    }

    public void Hurt() {
        health--;
        if (health <= 0) {
            Die();
        }
    }

    
    public void Killed(params string[] tags) {
        foreach (var t in tags) {
            if (!killCounts.ContainsKey(t)) killCounts[t] = 0;
            killCounts[t]++;
        }
    }

    void Die() {
        //TODO:kill player
        Debug.Log("KILLED!");
    }
    void OnGUI() {
        var style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        if (Event.current.type != EventType.repaint) return;
        //draw health
        var healthTex = healthIcons[Mathf.Clamp(health, 0, healthIcons.Length-1)];
        GUI.DrawTexture(healthRect, healthTex, ScaleMode.ScaleToFit, true);

        foreach (var counter in killCounters) {
            int count;
            if (!killCounts.TryGetValue(counter.name, out count)) count = 0;
            var content = new GUIContent(count.ToString(), counter.icon);
            GUI.DrawTexture(counter.position, counter.icon, ScaleMode.ScaleToFit, true);
            style.Draw(counter.textPos, count.ToString(), false, false, false, false);
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
