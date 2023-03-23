using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace PCG
{
    public class PCG : MonoBehaviour
    {

        [Header("Objects")]
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tile _tileSand;
        [SerializeField] private Tile _tileGrass;
        
        [Header("Generation size")]
        [SerializeField] private int _tileSizeX = 10;
        [SerializeField] private int _tileSizeY = 10;
        
        [Header("Perlin generation")]
        [Range(0,200)] [SerializeField] private float _perlinScale = 10f;
        [Range(0,1)] [SerializeField] private float _perlinThreshold = 0.5f;

        public void EmptyMap()
        {
            _tilemap.ClearAllTiles();
        }
        
        public void FillMap_NativeRandom()
        {
            
            EmptyMap();
            
            for (int x = -1 * _tileSizeX; x < _tileSizeX; x++)
            {
                for (int y = -1 * _tileSizeY; y < _tileSizeY; y++)
                {
                    float rnd = Random.Range(0, 2);
                    // Debug.Log("Tirage aléatoire : " + rnd);
                    
                    if(rnd >= 1)
                        _tilemap.SetTile(new Vector3Int(x, y, 0), _tileGrass);
                    else
                        _tilemap.SetTile(new Vector3Int(x, y, 0), _tileSand);
                    
                }
            }
        }

        public void FillMap_PerlinNoise()
        {
            EmptyMap();
            
            for (int x = -1 * _tileSizeX; x < _tileSizeX; x++)
            {
                for (int y = -1 * _tileSizeY; y < _tileSizeY; y++)
                {
                    float noiseCoordX = x / (2f * _tileSizeX) + 0.5f;
                    float noiseCoordY = y / (2f * _tileSizeY) + 0.5f;
                    
                    float rnd = Mathf.PerlinNoise(noiseCoordX * _perlinScale,  noiseCoordY * _perlinScale);
                    // Debug.Log("X:" + noiseCoordX + " - Y:" + noiseCoordY +" - Perlin Noise : " + rnd);
                    
                    if(rnd >= _perlinThreshold)
                        _tilemap.SetTile(new Vector3Int(x, y, 0), _tileGrass);
                    else
                        _tilemap.SetTile(new Vector3Int(x, y, 0), _tileSand);

                }
            }
        }

        
    }
}
