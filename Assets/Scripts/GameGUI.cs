using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GameGUI : MonoBehaviour {

    public Texture[] healthIcons;
    public Rect healthRect0;
    public float healthPadding = 10;

    public GUIKillCount[] killCounters;
    public int health;
    public int healthJars;
    public static GameGUI instance;
    public GameObject deathPrefab;
    public GameObject winPrefab;

    Dictionary<string, int> killCounts = new Dictionary<string, int>();


    public void AddBottle() {
        if (healthJars >= 10) return;
        healthJars++;
    }

    bool won;
    void Win() {
        if (won) return;
        won = true;
        winPrefab.Duplicate(FaceMoveControl.instance.transform.position);
    }
    void Awake() {
        instance = this;
    }

    public void Hurt(int amount) {
        health -= amount;
        if (health <= 0) {
            Die();
        }
    }

    public void Heal(int amount) {
        health += amount;
        if (health > healthJars * 4) health = healthJars * 4;
        if (health == 40) Win();
    }

    
    public void Killed(params string[] tags) {
        foreach (var t in tags) {
            if (!killCounts.ContainsKey(t)) killCounts[t] = 0;
            killCounts[t]++;
        }
    }

    void Die() {
        var obj = FaceMoveControl.instance;
        if (obj) {
            deathPrefab.Duplicate(obj.transform.position, obj.transform.rotation);
            Destroy(obj.gameObject);
        }
    }
    void OnGUI() {
        var style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        if (Event.current.type != EventType.repaint) return;
        //draw health
        for (int i = 0; i < healthJars; ++i) {
            var healthRect = healthRect0;
            healthRect.x += (healthRect0.width + healthPadding) * i;
            var hp = health - 4 * i;
            var healthTex = healthIcons[Mathf.Clamp(hp, 0, healthIcons.Length - 1)];
            GUI.DrawTexture(healthRect, healthTex, ScaleMode.ScaleToFit, true);
        }

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
