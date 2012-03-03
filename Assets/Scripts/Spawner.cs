using UnityEngine;
using System.Collections.Generic;
using LibNoise.Unity.Generator;
using System.Linq;

public class Spawner : MonoBehaviour {
    public SpawnInfo[] spawnables;
    public int spawnRadius = 7;
    public int despawnRadius = 10;

    List<SpawnData> spawned = new List<SpawnData>();
    Perlin noise;

    float Height(float x, float y) {
        var h = TileRenderer.Height(x, y);
        var g = (float)noise.GetValue(new Vector3(x/2, 0, y/2));
        g = Mathf.Abs(g);
        if (h <= 0) return 0f;
        return g;
    }
    void Start() {
        noise = new Perlin(1.0, 2.0, 0.5, 6, Random.Range(int.MinValue, int.MaxValue), LibNoise.Unity.QualityMode.Medium);
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
                Destroy(s.obj);
                return true;
            }
            return false;
        });
    }

    void TrySpawn(int x, int y) {

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
        if (height < 0.6f) return null;
        var rand = ((int)(height * 10000)) % spawnables.Length;

        return spawnables[rand];
    }

    struct SpawnData {
        public GameObject obj;
        public int x, y;

        public int Dist(int xpos, int ypos) {
            var d0 = Mathf.Abs(xpos - x) + Mathf.Abs(ypos - y);
            var d1 = Mathf.Abs(xpos - obj.transform.position.x) + Mathf.Abs(ypos - obj.transform.position.y);
            return Mathf.Min(d0, (int)d1);
        }
    }
}
[System.Serializable]
public class SpawnInfo {
    public GameObject prefab;
}
