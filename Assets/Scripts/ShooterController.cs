using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.AI;
using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using UnityEditor;

public class ShooterController : MonoBehaviour
{
    public GameObject bulletPrefab;
    Vector2 mouseDirection;
    public LineRenderer lineRenderer;
    public Transform lineOrigine;
    List<Tuple<Vector2, Vector2>> path; // first Vector EndPoint, Second Vector direction.
    public float power;
    public static int SelectedLayer;
    public Transform rayOrigin; 
    public float rotationSpeed = 100f;
    public static GameObject firedBullet;
    public static List<Vector2> destroyedCords = new();
  

    private void Start()
    {
        bulletPrefab.GetComponent<Rigidbody2D>();
       
    }

    private void MouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = (mousePosition - rayOrigin.position).normalized;
        transform.up = mouseDirection;
        Tile.selectedTile = RayUntilHitTile();
        if (Tile.selectedTile == null)
        {
            return;
        }
        else
        {
            int mask = Tile.selectedTile.gameObject.layer;
            int layerToMask = ~(1 << mask);
            RayUntilHitBottom(layerToMask);
        
        }
    }
    private void MouseUp()
    {      
        Shoot(path);
       lineRenderer.positionCount = 0;  
    }

    void Shoot(List<Tuple<Vector2, Vector2>> path)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.up = path[0].Item2;
        bullet.GetComponent<BulletScript>().Shoot(path, SelectedLayer);
        firedBullet = bullet;
        
    }

    void Update()
    {
   
        if (firedBullet != null) return;
        if (Input.GetMouseButton(0))
        {
            MouseDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
     
     

    }
    
    void RayUntilHitBottom(int layerToMask)
    {

        Vector2 origin = rayOrigin.position;
        Vector2 direction = mouseDirection;
        path = new()
        {
           
            new(origin, direction)
        };
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, origin);
        int i = 0;
        while (i < 50)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, float.PositiveInfinity, layerToMask);
            if(!hitInfo) break;
            Debug.DrawLine(origin, hitInfo.point);
            path.Add(new(hitInfo.point, direction));
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);
            if (hitInfo.transform.tag == "Destroy")
            {
                break;
            }
            direction = Vector2.Reflect(direction, hitInfo.normal);
            // adding a little offset so starting point dont get inside the collider.
            origin = hitInfo.point + (.001f * hitInfo.normal);
            i++;
        }

        
    }

    Tile RayUntilHitTile()
    {
        Vector2 origin = rayOrigin.position;
        Vector2 direction = mouseDirection;
        int i = 0;
        // 100 bounces
        //lineRenderer.positionCount = 1;
        //lineRenderer.SetPosition(0, origin);
        while (i < 50)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction);
            
            Tile tile;
            hitInfo.transform.TryGetComponent(out tile);
            if (tile)
            {
                SelectedLayer = tile.gameObject.layer;
            return tile;
            }

            direction = Vector2.Reflect(direction, hitInfo.normal);

            // adding a little offset so starting point dont get inside the collider.
            origin = hitInfo.point + (.001f * hitInfo.normal);
                //lineRenderer.positionCount++;
                //lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);
            i++;
        }
        return null;
    }




 
}

   










