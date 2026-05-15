using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static event Action<int> OnTutorialEnd;

    [Header("Effects variables")]
    [SerializeField] float cameraZoomInDuration;
    [SerializeField] float cameraOrthographicTarget;
    [SerializeField] AnimationCurve cameraMoveTrajectory;


    [Header("Tutorial variables")]
    [SerializeField] WaveSpawner waveSpawner;
    [SerializeField] WolfSpawner wolfSpawner;

    [SerializeField] int numberOfTutorials;

    [SerializeField] List<Transform> sheepSpawnPoint;
    [SerializeField] List<Transform> holeSpawnPoint;
    [SerializeField] Transform fenceSpawnPoint;
    [SerializeField] Transform wolfSpawnPoint;

    [SerializeField] List<GameObject> sheepTutorial;
    [SerializeField] List<GameObject> holeTutorial;
    [SerializeField] GameObject fenceTutorial;
    [SerializeField] GameObject wolfTutorial;


    [SerializeField] List<GameObject> tutorialAnimations;

    [SerializeField] CinemachineCamera mainCamera;

    int tutorialIndex = 0;

    bool IsTutorialComplete;

    private void Awake()
    {
        tutorialIndex = 0;
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += ZoomInCamera;

        GameManager.OnStartGame += PlayTutorial;
        Cage.OnAnimalCapturedInCorrectCage += AdvanceTutorial;
        Wolf.OnWolfStunned += EndTutorial;

    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= ZoomInCamera;

        GameManager.OnStartGame -= PlayTutorial;
        Cage.OnAnimalCapturedInCorrectCage -= AdvanceTutorial;
        Wolf.OnWolfStunned -= EndTutorial;

    }


    #region Tutorial play methods
    void PlayTutorial()
    {

        if (IsTutorialComplete) return;

        switch (tutorialIndex)
        {
            case 0:
                SpawnSheep(tutorialIndex);
                SpawnHole(tutorialIndex);
                break;
            case 1:
                SpawnSheep(tutorialIndex);
                SpawnFence(tutorialIndex);
                SpawnHole(tutorialIndex);
                break;
            case 2:
                SpawnWolf();
                break;
            default:
                break;
        }

        Debug.Log("Tutorial index: " + tutorialIndex);
        tutorialAnimations[tutorialIndex].gameObject.SetActive(true);


    }

    void AdvanceTutorial(Animal animal, Cage cage)
    {

        if (IsTutorialComplete) return;

        Debug.Log("Tutorial index: "+ tutorialIndex);
        tutorialAnimations[tutorialIndex].gameObject.SetActive(false);

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }


        tutorialIndex++;

        if (tutorialIndex >= numberOfTutorials)
        {
            EndTutorial();
        }


        PlayTutorial();
    }



    void SpawnSheep(int tutorialIndex)
    {

        GameObject entityToBeSpawned = sheepTutorial[tutorialIndex];

        GameObject entitySpawned = Instantiate(entityToBeSpawned, sheepSpawnPoint[0].position, Quaternion.identity);
        entitySpawned.transform.SetParent(this.transform);


        waveSpawner.RaiseEntitySpawnedEvent(entitySpawned);
    }


    void SpawnHole(int tutorialIndex)
    {

        GameObject entityToBeSpawned = holeTutorial[tutorialIndex];

        GameObject entitySpawned = Instantiate(entityToBeSpawned, holeSpawnPoint[0].position, Quaternion.identity);
        entitySpawned.transform.SetParent(this.transform);


        waveSpawner.RaiseEntitySpawnedEvent(entitySpawned);
    }

    void SpawnFence(int tutorialIndex)
    {

        GameObject entityToBeSpawned = fenceTutorial;

        GameObject entitySpawned = Instantiate(entityToBeSpawned, fenceSpawnPoint.position, Quaternion.identity);
        entitySpawned.transform.SetParent(this.transform);


    }


    void SpawnWolf()
    {

        GameObject entityToBeSpawned = wolfTutorial;

        GameObject wolfSpawned = Instantiate(entityToBeSpawned, wolfSpawnPoint.position, Quaternion.identity);

        wolfSpawned.GetComponent<Wolf>().SetPathfindingVariables(null, null);
        wolfSpawned.transform.SetParent(this.transform);

        wolfSpawner.RaiseWolfSpawnedEvent();


    }


    void EndTutorial()
    {
        StartCoroutine(DelayBeforeTutorialEndRoutine());
    }

    IEnumerator DelayBeforeTutorialEndRoutine()
    {
        yield return new WaitForSeconds(1f);

        IsTutorialComplete = true;
        OnTutorialEnd?.Invoke(0);

        Debug.Log("Setting tutorial manager false");
        this.gameObject.SetActive(false);
    }

    #endregion



    #region camera and post processing methods


    void ZoomInCamera()
    {
        StartCoroutine(ZoomInCameraRoutine());
    }



    IEnumerator ZoomInCameraRoutine()
    {
        float timePassed = 0f;
        LensSettings lens = mainCamera.Lens;
        float startSize = lens.OrthographicSize;
        float targetSize = cameraOrthographicTarget;

        while (timePassed < cameraZoomInDuration)
        {
            timePassed += Time.deltaTime;
            float t = Mathf.Clamp01(timePassed / cameraZoomInDuration);
            float curveT = cameraMoveTrajectory.Evaluate(t);

            lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, curveT);
            mainCamera.Lens = lens;

            yield return null;
        }

        lens.OrthographicSize = targetSize;
        mainCamera.Lens = lens;
    }


    #endregion


}
