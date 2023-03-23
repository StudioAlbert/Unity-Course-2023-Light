using System;
using System.Collections;
using System.Collections.Generic;
using PCGDungeon;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrisjkaMapMouseFollower : MonoBehaviour
{

    [SerializeField] private Tilemap _hitMap;
    [SerializeField] private Tilemap _drawMap;
    [SerializeField] private TileBase _startingPositionTile;
    [SerializeField] private TileBase _roughPathTile;
    [SerializeField] private TileBase _randomPathTile;
    [SerializeField] private TileBase _optimizedTile;
    [SerializeField] private DrisjkaMapGenerator _generator;

    private Vector2Int startPoint = Vector2Int.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);

        RaycastHit2D hitInfo = Physics2D.Raycast(
            new Vector2(ray.origin.x, ray.origin.y),
            new Vector2(ray.direction.x, ray.direction.y)
            );


        if (hitInfo.collider != null)
        {
            Debug.Log("You hit the map !");

            Vector2Int hitPoint = new Vector2Int((int)hitInfo.point.x, (int)hitInfo.point.y);
            
            if (hitPoint != startPoint)
            {
                startPoint = hitPoint;
                // List<Vector2Int> path = new List<Vector2Int>();
        
                // Now draw it ! -----------------------------------------------------------
                _drawMap.ClearAllTiles();
                MapPainter.CellPaint(startPoint, _drawMap, _startingPositionTile);
                
                foreach (var point in _generator.GetAPath(startPoint, false))
                    MapPainter.CellPaint(point, _drawMap, _roughPathTile);
                                
                foreach (var point in _generator.GetAPath(startPoint, true))
                    MapPainter.CellPaint(point, _drawMap, _randomPathTile);
            }
            
        }
        else
        {
            Debug.Log(hitInfo.ToString());
        }

    }

}
