using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color originalColor;
    public Color color
    {
        get => myRenderer.material.color;
        set
        {
            myRenderer.material.color = value;
            if (originalColor == null) originalColor = value;
        }

    }
    public static Tile selectedTile = null;
    public List<Tile> myNeighbours = new List<Tile>();
    public List<Tile> friendlyNeigbours = new List<Tile>();
    public Action<Tile> OnDrag;
    public Action<Tile> OnHover;

    public Bounds myBounds;
    private SpriteRenderer myRenderer;

    public Vector2 myGridCords;

    private void Awake()
    {
        myBounds = GetComponent<Collider2D>().bounds;
        myRenderer = GetComponent<SpriteRenderer>();
    }

    //private void OnMouseDrag()
    //{
    //    if (Input.GetMouseButton(0))
    //        OnDrag?.Invoke(this);
    //}

    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButton(0))
    //        OnHover?.Invoke(this);
    //    /*if (!(Input.GetMouseButton(0) && GridManager.currentBall != null)) return;
    //    Debug.Log("BT_A1::" + name);
    //    if (GridManager.currentBall == null) GridManager.currentBall = this;
    //    else if (GridManager.currentBall.color == color)
    //    {
    //        color = Color.black;
    //        GridManager.selectedBalls.Add(this);
    //    }
    //    else
    //    {
    //        GridManager.currentBall = null;
    //        GridManager.selectedBalls = new List<Ball>();
    //    }*/
    //}
   void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Destroy"))
        {
            
            
                Destroy(gameObject);
            
        }
    }
    public void MakeFriends()
    {
        foreach (Tile tile in myNeighbours)
        {
            if (tile.originalColor == originalColor)
            {
                friendlyNeigbours.Add(tile);
            }
        }
    }
    public void ClearNeighbours()
    {
        myNeighbours = new();
        friendlyNeigbours = new();
    }

}
