using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Random = UnityEngine.Random;

public class BoatSpawner : MonoBehaviour
{
    [SerializeField] private PathCreator[] fishPaths;
    [SerializeField] private PathCreator[] cargoCruisePaths;
    
    [SerializeField] private GameObject fishingBoat;
    [SerializeField] private GameObject cruiseShip;
    [SerializeField] private GameObject cargoShip;
    
    [SerializeField] private int numberOfShipsPerPath = 5;
    
    private void Start()
    {
        Transform shipParent = new GameObject("Ships").transform;
        
        foreach (var path in fishPaths)
        {
            float distanceToBeSpawned = 0;
            for (int i = 0; i < numberOfShipsPerPath; i++)
            {
                SpawnShip(path, fishingBoat, distanceToBeSpawned, shipParent);
                distanceToBeSpawned += path.path.length / numberOfShipsPerPath;
            }
        }

        foreach (var path in cargoCruisePaths)
        {
            float distanceToBeSpawned = 0;
            for (int i = 0; i < numberOfShipsPerPath; i++)
            {
                int flag = Random.Range(0, 2);
                GameObject prefab = (flag == 1) ? cargoShip : cruiseShip;
                SpawnShip(path, prefab, distanceToBeSpawned, shipParent);
                distanceToBeSpawned += path.path.length / numberOfShipsPerPath;
            }
        }
    }

    private void SpawnShip(PathCreator path, GameObject prefab, float distanceToBeSpawned, Transform parent = null)
    {
        GameObject ship = Instantiate(prefab);
        ship.GetComponent<Boat>().Initialize(distanceToBeSpawned, path);
        ship.transform.parent = parent;
    }
}