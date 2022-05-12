using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    public TerrainType terrainType;
}


[System.Serializable]
public enum TerrainType
{
    Grass,
    Road,
    Water,
    MovingBlock
}