using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Driska Settings", menuName = "PCG/Drisjka Settings")]
public class DrisjkaMapSettings : ScriptableObject
{
    [Header("Path calculator")]
    public int MaxPathLength;
    // public bool RandomNeighbour;
    
}
