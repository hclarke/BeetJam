using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileRenderer : MonoBehaviour {

    public Tile[] tileMap;
    Tile[] tile_map;

    public GameObject tilePrefab;
    Material base_material;
    Dictionary<Texture2D, Material> materials = new Dictionary<Texture2D, Material>();

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
        for (int i = 0; i < tileMap.Length; ++i) {
            for (int j = 0; j < 4; ++j) {
                tile_map[i * 4 + j] = tileMap[i].Rotate(j);
            }
        }
        base_material = tile.GetComponentInChildren<Renderer>().sharedMaterial;
        Destroy(tile);

        var tiles = GetTiles(10, 10);
        Layout(tiles);
    }

    TileType[,] GetTiles(int width, int height) {
        var heights = new float[width,height];
        for(int i = 0; i < width; ++i) {
            for(int j = 0; j < height; ++j) {
                heights[i, j] = Mathf.PerlinNoise(i * 1f / width, j * 1f / height) * 2 - 0.5f;
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

    void CreateTile(Tile t, int x, int y) {
        var rot = Quaternion.AngleAxis(t.rotation * 90, Vector3.up);
        var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), rot) as GameObject;
        tile.GetComponentInChildren<Renderer>().material = GetMaterial(t.texture);
        tile.transform.parent = transform;
        if (t.NE == TileType.Water) CreateBox(x, y, 0).parent = tile.transform;
        if (t.SE == TileType.Water) CreateBox(x, y, 1).parent = tile.transform;
        if (t.SW == TileType.Water) CreateBox(x, y, 2).parent = tile.transform;
        if (t.NW == TileType.Water) CreateBox(x, y, 3).parent = tile.transform;
    }

    Transform CreateBox(float x, float y, int rot) {
        var go = new GameObject("blocker " + rot);
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

        go.transform.position = new Vector3(x, 0, y);
        var box = go.AddComponent<BoxCollider>();
        box.center = Vector3.zero;
        box.size = new Vector3(0.5f, 1, 0.5f);
        return box.transform;
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