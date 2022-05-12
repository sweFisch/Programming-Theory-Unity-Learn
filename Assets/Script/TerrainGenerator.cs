using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] private Terrain[] _prefabTerrain;

    [SerializeField] private TerrainType[] spawnTerrains;

    // Pooling of enemies , terrain, and moving plattforms ect



    // this is for dooing spawning terrain in a different way where you save the position and can continue to sapwn more lanes
    private Vector3 currentSpawnPosition = new Vector3(0,-0.5f,0);
    // the point is to save a reference to the position so you can generate more of the level each jump
    // Maybee have different types of road, grass water combinations you can spawn in in differnet random loops
    // rated on difficulty, intensity ect and ramp upp during play

    // in dapper dinos video he had a scriptable object that defined the max lanes of the same type that could be spawned in a row 5 is probably a good number
    // the lanes traffic and moving plattforms should spawn in an alternating direction pattern

    private void Start()
    {
        // spawn block of terrain defined in the variable array
        for (int i = 0; i < spawnTerrains.Length; i++)
        {
            foreach (var terrain in _prefabTerrain)
            {
                if(spawnTerrains[i] == terrain.terrainType)
                {
                    Instantiate(terrain.gameObject, new Vector3(0f, -0.5f, i), Quaternion.identity, transform);
                }
            }
        }
    }


    // Layared spawnning spawning in hazards on roads, and water, and blockers on grass


    // spawn coins in free areas.
}
