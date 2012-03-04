using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LibNoise.Unity.Generator;

public class TileRenderer : MonoBehaviour {

    public Tile[] tileMap;
    Tile[] tile_map;

    public GameObject tilePrefab;

    public GameObject waterPrefab;
    Material base_material;
    Dictionary<Texture2D, Material> materials = new Dictionary<Texture2D, Material>();
    static Perlin noise;

    Stack<GameObject> water_pool = new Stack<GameObject>();

    public static float Height(float x, float y) {
        var val = (float)noise.GetValue(new Vector3(x/20f, 0f, y/20f));
        val = Mathf.Pow(val, 2) - 0.05f;
        return val;
    }

    public static bool OnWaterTile(float x, float y) {
        var xi = Mathf.RoundToInt(x-0.5f);
        var yi = Mathf.RoundToInt(y-0.5f);
        return Height(xi, yi) <= 0f;
    }
    
    GameObject GetWater() {
        if (water_pool.Count == 0) {
            var go = waterPrefab.Duplicate();
            go.name = waterPrefab.name;
            return go;
        }
        var water = water_pool.Pop();
        water.SetActiveRecursively(true);
        return water;
    }

    void FreeWater(GameObject water) {
        water_pool.Push(water);
    }

    Material GetMaterial(Texture2D tex ) {
        Material mat;
        if(!materials.TryGetValue(tex, out mat)) {
            mat = new Material(base_material);
            mat.mainTexture = tex;
            materials[tex] = mat;
        }
        return mat;
    }

    
    void Start() {
        var tile = Instantiate(tilePrefab) as GameObject;
        tile_map = new Tile[tileMap.Length * 4];

        noise = new LibNoise.Unity.Generator.Perlin(1.0, 2.0, 0.5, 6, Random.Range(int.MinValue, int.MaxValue), LibNoise.Unity.QualityMode.Medium);
        for (int i = 0; i < tileMap.Length; ++i) {
            for (int j = 0; j < 4; ++j) {
                tile_map[i * 4 + j] = tileMap[i].Rotate(j);
            }
        }
        base_material = tile.GetComponentInChildren<Renderer>().sharedMaterial;
        Destroy(tile);
    }

    List<GameObject> live_tiles = new List<GameObject>();
    
    void Update() {
        var range = 5;
        var x = Mathf.RoundToInt(transform.position.x);
        var y = Mathf.RoundToInt(transform.position.z);

        foreach (var t in live_tiles) {
            if (!t) continue;
            var tx = Mathf.RoundToInt(t.transform.position.x);
            var ty = Mathf.RoundToInt(t.transform.position.z);
            if (Mathf.Abs(x - tx) > range || Mathf.Abs(y - ty) > range) {
                foreach (Transform trans in t.transform) {
                    var g = trans.gameObject;
                    if (g.name == waterPrefab.name) {
                        trans.parent = null;
                        g.SetActiveRecursively(false);
                        FreeWater(g);
                    }
                }
                Destroy(t);
            }
        }
        live_tiles.RemoveAll(t => !t);

        for (int i = x - range; i <= x + range; ++i) {
            for (int j = y - range; j <= y + range; ++j) {
                if (!live_tiles.Any(tile => {
                    var tx = Mathf.RoundToInt(tile.transform.position.x);
                    var ty = Mathf.RoundToInt(tile.transform.position.z);
                    return tx == i && ty == j;
                })) {
                    var t = LayoutTile(i, j);
                    live_tiles.Add(t);
                }
            }
        }
    }

    float[,] temp_height = new float[3, 3];
    TileType[,] temp_types = new TileType[3, 3];
    TileType GetTile(int x, int y) {
        for (int i = -1; i < 2; ++i) {
            for (int j = -1; j < 2; ++j) {
                var posx = x + i;
                var posy = y + j;
                //temp_height[i+1, j+1] = (float)noise.GetValue(posx * 1f / 20, 0.0, posy * 1f / 20);
                //temp_height[i+1, j+1] = Mathf.Pow(temp_height[i+1, j+1], 2) - 0.05f;
                temp_height[i + 1, j + 1] = Height(posx, posy);
            }
        }

        for (int i = -1; i < 2; ++i) {
            for (int j = -1; j < 2; ++j) {
                if (temp_height[i+1, j+1] > 0f) temp_types[i+1, j+1] = TileType.Grass;
                else temp_types[i+1, j+1] = TileType.Water;
            }
        }

        if (temp_types[1, 1] == TileType.Grass) {
            if (
                IsWater(temp_types, 0, 0) ||
                IsWater(temp_types, 1, 0) ||
                IsWater(temp_types, 2, 0) ||
                IsWater(temp_types, 0, 2) ||
                IsWater(temp_types, 1, 2) ||
                IsWater(temp_types, 2, 2) ||
                IsWater(temp_types, 0, 1) ||
                IsWater(temp_types, 2, 1)) {
                temp_types[1, 1] = TileType.Sand;
            }

        }

        return temp_types[1, 1];

    }

    GameObject LayoutTile(int x, int y) {
        var ts = GetTiles(x - 2, x + 4, y - 2, y + 4);
        var sw = ts[1,1];//GetTile(x, y);
        var nw = ts[1,2];//GetTile(x, y + 1);
        var ne = ts[2,2];//GetTile(x + 1, y + 1);
        var se = ts[2, 1];//GetTile(x + 1, y + 1);
        var tile = GetTile(ne, se, sw, nw);

        return CreateTile(tile, x, y);
    }

    TileType[,] GetTiles(int xmin, int xmax, int ymin, int ymax) {
        var width = xmax - xmin;
        var height = ymax - ymin;
        var heights = new float[width,height];
       
        for(int i = 0; i < width; ++i) {
            for(int j = 0; j < height; ++j) {
                var x = xmin + i;
                var y = ymin + j;
                heights[i, j] = (float)noise.GetValue(x * 1f / 20, 0.0, y * 1f / 20);
                heights[i, j] = Mathf.Pow(heights[i, j], 2) - 0.05f;
            }
        }
        //heights[1, 1] = -1f;
        var tiles = new TileType[width, height];
        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                if (heights[i, j] > 0f) tiles[i, j] = TileType.Grass;
                else tiles[i, j] = TileType.Water;
            }
        }

        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                if (tiles[i, j] == TileType.Grass) {
                    if (
                        IsWater(tiles, i - 1, j - 1) ||
                        IsWater(tiles, i - 0, j - 1) ||
                        IsWater(tiles, i + 1, j - 1) ||
                        IsWater(tiles, i - 1, j + 1) ||
                        IsWater(tiles, i - 0, j + 1) ||
                        IsWater(tiles, i + 1, j + 1) ||
                        IsWater(tiles, i - 1, j - 0) ||
                        IsWater(tiles, i + 1, j - 0)) {
                            tiles[i, j] = TileType.Sand;
                    }

                }
            }
        }
        return tiles;
    }

    bool IsWater(TileType[,] tiles, int x, int y) {
        if (x < 0 || x > tiles.GetUpperBound(0)) return false;
        if (y < 0 || y > tiles.GetUpperBound(1)) return false;
        return tiles[x, y] == TileType.Water;
    }

    void Layout(TileType[,] tiles) {
        var width = tiles.GetLength(0)-1;
        var height = tiles.GetLength(1)-1;
        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                var sw = tiles[i, j];
                var nw = tiles[i, j + 1];
                var ne = tiles[i + 1, j + 1];
                var se = tiles[i + 1, j];
                var tile = GetTile(ne, se, sw, nw);

                CreateTile(tile, i, j);

            }
        }
    }

    GameObject CreateTile(Tile t, int x, int y) {
        var rot = Quaternion.AngleAxis(t.rotation * 90, Vector3.up);
        var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), rot) as GameObject;
        //tile.isStatic = true;
        tile.GetComponentInChildren<Renderer>().material = GetMaterial(t.texture);
        //tile.transform.parent = transform;
        if (t.NE == TileType.Water) CreateBox(x, y, 0).parent = tile.transform;
        if (t.SE == TileType.Water) CreateBox(x, y, 1).parent = tile.transform;
        if (t.SW == TileType.Water) CreateBox(x, y, 2).parent = tile.transform;
        if (t.NW == TileType.Water) CreateBox(x, y, 3).parent = tile.transform;
        return tile;
    }

    Transform CreateBox(float x, float y, int rot) {
        
        //go.isStatic = true;
        switch (rot) {
            case 0:
                x += 0.25f; y += 0.25f;break;
            case 1:
                x += 0.25f; y -= 0.25f; break;
            case 2:
                x -= 0.25f; y -= 0.25f; break;
            case 3:
                x -= 0.25f; y += 0.25f; break;
            default:
                Debug.LogError("this isn't good."); break;
        }
        var go = GetWater();


        go.transform.position = new Vector3(x, 0, y);
        return go.transform;
    }

    Tile GetTile(TileType NE, TileType SE, TileType SW, TileType NW) {
        var tile = tile_map.FirstOrDefault(t =>
            t.NE == NE &&
            t.SE == SE &&
            t.SW == SW &&
            t.NW == NW);
        if (tile == null) {
            Debug.LogError(string.Format("no tile of type: [{0},{1},{2},{3}]", NE, SE, SW, NW));
        }
        return tile;
    }
}

[System.Serializable]
public class Tile {
    public TileType NE, SE, SW, NW;
    public Texture2D texture;
    public int rotation;

    public Tile Rotate(int r) {
        var t = new Tile();
        t.texture = texture;
        t.rotation = r;

        switch (r) {
            case 0: 
                t.NE = NE; t.SE = SE; t.SW = SW; t.NW = NW; break;
            case 1:
                t.NE = NW; t.SE = NE; t.SW = SE; t.NW = SW; break;
            case 2:
                t.NE = SW; t.SE = NW; t.SW = NE; t.NW = SE; break;
            case 3:
                t.NE = SE; t.SE = SW; t.SW = NW; t.NW = NE; break;
            default:
                Debug.LogError("this isn't good."); break;
        }
        return t;
    }
}

public enum TileType {
    Grass,
    Sand,
    Water
}