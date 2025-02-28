using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
public class BulletScript : MonoBehaviour
{

    public float speed = 1;
    List<Tuple<Vector2, Vector2>> path; // first Vector EndPoint, Second Vector direction.
    int selectedLayer;
  
    public void Shoot(List<Tuple<Vector2, Vector2>> path, int selectedLayer)
    {
        this.path = new List<Tuple<Vector2, Vector2>>();
        this.selectedLayer = selectedLayer;
        this.path.AddRange(path);
        FollowPath(1);

    }
    void FollowPath(int index)
    {
        if(index < path.Count)
        {
            float distance = Vector2.Distance(path[index -1].Item1, path[index].Item1 );
            float time = distance / speed;
            transform.up = path[index].Item2;
            transform.DOMove(path[index].Item1, time).SetEase(Ease.Linear).onComplete += ()=> FollowPath(index + 1);
        }
        else
        {
            Dictionary<int, int> destroyedCords = new();
            destroyedCords = this.destroyedCords;
            GridManager.onBulletDestroyed?.Invoke(destroyedCords);
            this.destroyedCords.Clear();
            
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Destroy"))
        {
            Dictionary<int, int> destroyedCords = new();
            destroyedCords = this.destroyedCords;
            GridManager.onBulletDestroyed?.Invoke(destroyedCords);
            this.destroyedCords.Clear();
           
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Tile"))
        {
            if(collision.gameObject.layer == selectedLayer)
            {
                Vector2 cords = collision.gameObject.GetComponent<Tile>().myGridCords;

                ResolveCords(cords);
                Destroy(collision.gameObject);
            }
        }
      
    }
   
 
    Dictionary<int, int> destroyedCords = new();
    void ResolveCords(Vector2 cord)
    {
        
        if (!destroyedCords.ContainsKey((int)cord.x))
        {
            
            destroyedCords.Add((int)cord.x, (int)cord.y);
        }
        else
        {
            if (cord.y < destroyedCords[(int)cord.x])
            {
                destroyedCords[(int)cord.x] = (int)cord.y;
            }
        }
    }


}
