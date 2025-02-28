using DG.Tweening;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    public Tile _tilePrefab;
    public Tile _tilePrefabGreen;
    public Tile _tilePrefabBlue;
    public int gridX = 5;
    public int gridY = 5;
    public List<Color> tileColors = new List<Color>();

    public static List<Tile> allTiles = new();
    public static Dictionary<Vector2, Tile> _allTiles = new();
    public static Tile currentTile = null;
    public static List<Tile> selectedTiles = new List<Tile>();
    public static Tile[,] tiles = null;
    public Vector2[,] grid = null;

    Dictionary<Tile, List<Tile>> tilesGroup = new();

    int score = 0;
    private Vector3 initialPosition;
    public float distanceToMoveGrid = 0.5f;
    public static Action<Dictionary<int, int>> onBulletDestroyed;
    public void Start()
    {
        CreatePosGrid();
        CreateColorGroups();
        SetBallsToGrid();
        //MeetNeighbours();
        initialPosition = transform.position;
        onBulletDestroyed += OnBulletDestroyed;
        
    }

    private void Update()
    {

    }

    bool IsCombo() => selectedTiles.Count > 2;

    public void CreatePosGrid()
    {
        grid = new Vector2[gridX, gridY];
        Vector2 pos = new Vector2(1f, 1);
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                grid[i, j] = pos;
                pos += new Vector2(0, -0.1f);
            }
            pos.y = 1;
            pos += new Vector2(0.1f, 0);
        }
    }

    public void SetBallsToGrid()
    {
        tiles = new Tile[gridX, gridY];
        int index = 0;
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                Tile tile = allTiles[index];

                tile.myGridCords = new Vector2(i, j);
                tile.transform.localPosition = grid[i, j];
                tiles[i, j] = tile;
                index++;

            }
        }
    }

    void OnBulletDestroyed(Dictionary<int, int> cords)
    {
        foreach (var kvp in cords)
        {
            int x = kvp.Key;
            int y = kvp.Value;

            for (int j = y; j < gridY; j++)
            {
                if (tiles[x, j] != null)
                {
                    tiles[x, j].AddComponent<Rigidbody2D>();
                    tiles[x, j].gameObject.layer = 2;
                }

            }
        }
        //StartCoroutine(CallAfterBulletDestroyed(cords));    
    }

    public IEnumerator CallAfterBulletDestroyed(Dictionary<int, int> cords)
    {
        yield return new();
       
    }
    public void CreateColorGroups()
    {
        foreach (Tile tile in allTiles)
        {
            Destroy(tile.gameObject);
        }
        allTiles?.Clear();

        int groupSize = gridX * gridY / 3;
        for (int i = 0; i < groupSize; i++)
        {
            var tile = Instantiate(_tilePrefabGreen, transform);
            allTiles.Add(tile);
        }

        for (int i = 0; i < groupSize; i++)
        {
            var tile = Instantiate(_tilePrefab, transform);
            allTiles.Add(tile);
        }
        for (int i = 0; i < groupSize; i++)
        {
            var tile = Instantiate(_tilePrefabBlue, transform);
            allTiles.Add(tile);
        }
    }

    public void MeetNeighbours()
    {
        foreach (Tile tile in allTiles)
        {
            tile.ClearNeighbours();
        }
        for (int i = 0; i < gridX; i++)
        {
            for (int j = gridY; j < 2 * gridY; j++)
            {
                var tile = tiles[i, j];
                if (tile == null) continue;
                Tile left = !(i - 1 < 0) ? tiles[i - 1, j] : null;
                Tile upperLeft = !(i - 1 < 0) && !(j - 1 < gridY) ? tiles[i - 1, j - 1] : null;
                Tile upper = !(j - 1 < gridY) ? tiles[i, j - 1] : null;
                Tile upperRight = !(i + 1 > gridX - 1) && !(j - 1 < gridY) ? tiles[i + 1, j - 1] : null;
                Tile right = !(i + 1 > gridX - 1) ? tiles[i + 1, j] : null;
                Tile downRight = !(i + 1 > gridX - 1) && !(j + 1 > (2 * gridY) - 1) ? tiles[i + 1, j + 1] : null;
                Tile down = !(j + 1 > (2 * gridY) - 1) ? tiles[i, j + 1] : null;
                Tile downLeft = !(i - 1 < 0) && !(j + 1 > (2 * gridY) - 1) ? tiles[i - 1, j + 1] : null;

                if (left != null) tile.myNeighbours.Add(left);
                if (upperLeft != null) tile.myNeighbours.Add(upperLeft);
                if (upper != null) tile.myNeighbours.Add(upper);
                if (upperRight != null) tile.myNeighbours.Add(upperRight);
                if (right != null) tile.myNeighbours.Add(right);
                if (downRight != null) tile.myNeighbours.Add(downRight);
                if (down != null) tile.myNeighbours.Add(down);
                if (downLeft != null) tile.myNeighbours.Add(downLeft);

                tile.MakeFriends();
            }
        }

    }

    void ResetSelectedBalls()
    {
        selectedTiles?.Clear();
    }

    [ContextMenu("ResetGrid")]
    public void ResetGrid()
    {
        CreateColorGroups();
        //SetBallsToGrid();
        //MeetNeighbours();
    }




}