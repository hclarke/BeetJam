using UnityEngine;
using System.Collections.Generic;
using LibNoise.Unity.Generator;
using System.Linq;

public class Spawner : MonoBehaviour {
    public SpawnInfo[] spawnables;
    public int spawnRadius = 7;
    public int despawnRadius = 10;
    public float magic = 0.6f;
    public float moreMagic = 0.15f;

    List<SpawnData> spawned = new List<SpawnData>();
    Perlin noise;
    Dictionary<int, HashSet<int>> killed = new Dictionary<int, HashSet<int>>();

    float totalWeight;
    void AddKilled(int x, int y) {
        HashSet<int> set;
        if (!killed.TryGetValue(x, out set)) {
            set = new HashSet<int>();
            killed[x] = set;
        }
        set.Add(y);
    }

    bool WasKilled(int x, int y) {
        HashSet<int> set;
        if (killed.TryGetValue(x, out set)) {
            return set.Contains(y);
        }
        return false;
    }

    float Height(float x, float y) {
        var h = TileRenderer.Height(x, y);
        var g = (float)noise.GetValue(new Vector3(x*moreMagic, 0, y*moreMagic));
        g = Mathf.Abs(g);
        if (h <= 0) return 0f;
        return g;
    }
    void Start() {
        noise = new Perlin(1.0, 2.0, 0.5, 6, Random.Range(int.MinValue, int.MaxValue), LibNoise.Unity.QualityMode.Medium);
        totalWeight = spawnables.Sum(s => s.weight);
    }
    public void Update() {
        var x = Mathf.RoundToInt(transform.position.x);
        var y = Mathf.RoundToInt(transform.position.z);

        for (int i = -spawnRadius; i <= spawnRadius; ++i) {
            TrySpawn(i+x, spawnRadius+y);
            TrySpawn(i+x, -spawnRadius+y);
            TrySpawn(spawnRadius+x, i+y);
            TrySpawn(-spawnRadius+x, i+y);
        }

        spawned.RemoveAll(s => {
            var d = s.Dist(x, y);
            if (d > despawnRadius) {
                if (!s.obj) AddKilled(s.x, s.y);
                Destroy(s.obj);
                return true;
            }
            return false;
        });
    }

    void TrySpawn(int x, int y) {
        if (WasKilled(x, y)) return;
        if (spawned.Any(s => s.x == x && s.y == y)) return;

        var h = Height(x, y);
        var type = GetSpawnType(h);
        if (type == null) return;
        var go = type.prefab.Duplicate(x+0.5f, 0, y+0.5f);
        var data = new SpawnData() {
            obj = go,
            x = x,
            y = y,
        };
        spawned.Add(data);
    }

    SpawnInfo GetSpawnType(float height) {
        if (height < magic) return null;
        var rand = Mathf.Repeat(height * 5000, totalWeight);
        foreach (var s in spawnables) {
            if (rand <= s.weight) return s;
            rand -= s.weight;
        }

        throw new System.Exception("that's broken");
    }

    struct SpawnData {
        public GameObject obj;
        public int x, y;

        public int Dist(int xpos, int ypos) {
            var d0 = Mathf.Abs(xpos - x) + Mathf.Abs(ypos - y);
            if (!obj) return d0;
            var d1 = Mathf.Abs(xpos - obj.transform.position.x) + Mathf.Abs(ypos - obj.transform.position.y);
            return Mathf.Min(d0, (int)d1);
        }
    }
}
[System.Serializable]
public class SpawnInfo {
    public GameObject prefab;
    public float weight = 1f;
}
