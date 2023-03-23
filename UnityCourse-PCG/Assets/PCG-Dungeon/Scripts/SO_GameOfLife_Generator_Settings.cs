using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameOfLife Settings", menuName = "PCG/GameOfLife Settings")]
public class SO_GameOfLife_Generator_Settings : ScriptableObject
{
    [Header("Space Definition")]
    public Vector2Int size;
    [Range(0,1)] 
    public float bordersRatio = 0.0f;
    public Vector2Int startPosition;

    [Header("Parameters")]
    [Range(0,1)] 
    public float initialFillRatio = 0.5f;
    public int maxIterations = 200;
    public List<int> stayAliveValues;
    public List<int> comeAliveValues;
    
}
