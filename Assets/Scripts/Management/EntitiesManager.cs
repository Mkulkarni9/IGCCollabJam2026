using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    [SerializeField] PointerGrabber pointerGrabber;


    List<GameObject> entitiesSpawnedThisLevel = new List<GameObject>();


    private void OnEnable()
    {
        WaveSpawner.OnEntitySpawned += AddEntityToLevelCagesList;
        LevelManager.OnLevelComplete += ReleaseGrabbedAnimals;
        LevelManager.OnLevelComplete += DestroyAllRemainingEntities;
    }

    private void OnDisable()
    {
        WaveSpawner.OnEntitySpawned -= AddEntityToLevelCagesList;
        LevelManager.OnLevelComplete -= ReleaseGrabbedAnimals;
        LevelManager.OnLevelComplete -= DestroyAllRemainingEntities;


    }
    void AddEntityToLevelCagesList(GameObject entitySpawned)
    {
        entitiesSpawnedThisLevel.Add(entitySpawned);
    }

    void ReleaseGrabbedAnimals(int levelIndex)
    {
        pointerGrabber.ReleaseAnimal();
    }



    void DestroyAllRemainingEntities(int levelIndex)
    {
        foreach (GameObject entityRemaining in entitiesSpawnedThisLevel)
        {
            if (entityRemaining != null)
            {
                Destroy(entityRemaining);
            }

        }

        entitiesSpawnedThisLevel.Clear();
    }
}
