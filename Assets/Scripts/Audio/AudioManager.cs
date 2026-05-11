using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{

    [SerializeField] GameObject audioPrefab;

    [Header("SFX Clips")]
    [SerializeField] AudioSO hoverOnSheep;
    //[SerializeField]  List<AudioSO> sheepNoisesHoverOnSheep;

    [SerializeField] AudioSO grabSheep;
    [SerializeField] List<AudioSO> sheepNoisesGrabSheep;

    [SerializeField] AudioSO hoverOnHole;

    [SerializeField] AudioSO dropSheepInCorrectHole;
    [SerializeField] List<AudioSO> sheepNoisesDropSheepInCorrectHole;

    [SerializeField] AudioSO dropSheepInWrongHole;
    [SerializeField] List<AudioSO> sheepNoisesDropSheepInWrongHole;

    [SerializeField] AudioSO hitFence;

    [SerializeField] List<AudioSO> wolfNoisesSpawnWolf;

    [SerializeField] AudioSO stunWolf;
    [SerializeField] List<AudioSO> wolfNoisesStunWolf;


    [SerializeField] List<AudioSO> wolfNoisesEatSheep;




    [Header("Music Clips")]
    [SerializeField] AudioSO titleMusic;
    [SerializeField] AudioSO ambientMusic;



    IObjectPool<GameObject> audioPool;


    public float MusicVolume { get; private set; }



    #region Unity methods

    


    void OnEnable()
    {
        ObjectPoolingManager.OnObjectPoolManagerCreated += GetAudioPool;


        Animal.OnHoverPointer += PlayHoverOnSheepSFX;
        PointerGrabber.OnGrabbedSheep += PlayGrabSheepSFX;



    }



    void OnDisable()
    {
        ObjectPoolingManager.OnObjectPoolManagerCreated -= GetAudioPool;

        Animal.OnHoverPointer -= PlayHoverOnSheepSFX;
        PointerGrabber.OnGrabbedSheep -= PlayGrabSheepSFX;





    }

    private void Start()
    {
        PlayTitleMusic();
        PlayAmbientMusic();
    }
    #endregion


    #region setup audio methods
    void GetAudioPool()
    {
        audioPool = ObjectPoolingManager.Instance.GetPool(audioPrefab.gameObject.GetInstanceID(), audioPrefab);
    }


    void PlaySound(AudioSO audioSO, float clipVolume = 1)
    {
        //Debug.Log("AudioSO: " + audioSO);
        AudioClip audioClip = audioSO.AudioClip;


        GameObject soundObject = audioPool.Get();
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = audioSO.volume;
        audioSource.outputAudioMixerGroup = audioSO.mixer;

        if (audioSO.ChangePitch)
        {
            audioSource.pitch = Random.Range(audioSO.pitchRangeMin, audioSO.pitchRangeMax);
        }

        audioSource.PlayOneShot(audioSource.clip);
        StartCoroutine(ReturnToPoolAfterPlayRoutine(soundObject, audioClip.length));
    }
    IEnumerator ReturnToPoolAfterPlayRoutine(GameObject soundObject, float lengthOfClip)
    {
        yield return new WaitForSeconds(lengthOfClip);
        audioPool.Release(soundObject);
    }


    void PlayMusic(AudioSO audioSO)
    {

        GameObject musicObject = new GameObject("Music audio source");
        AudioSource audioSource = musicObject.AddComponent<AudioSource>();

        AudioClip musicClip = audioSO.AudioClip;

        audioSource.clip = musicClip;
        audioSource.loop = audioSO.loop;
        audioSource.volume = audioSO.volume;
        audioSource.outputAudioMixerGroup = audioSO.mixer;
        audioSource.Play();
    }

    #endregion

    #region Play SFX clips

    public void PlayHoverOnSheepSFX()
    {
        PlaySound(hoverOnSheep);


        /*int randomIndex = Random.Range(0,sheepNoisesHoverOnSheep.Count);

        PlaySound(sheepNoisesHoverOnSheep[randomIndex]);*/
    }

    public void PlayGrabSheepSFX(bool status)
    {
        if(status)
        {
            PlaySound(grabSheep);

            int randomIndex = Random.Range(0, sheepNoisesGrabSheep.Count);

            PlaySound(sheepNoisesGrabSheep[randomIndex]);
        }

    }
    public void PlayHoverOnHoleSFX()
    {
        PlaySound(hoverOnHole);
    }

    public void PlayDropSheepInCorrectHoleSFX()
    {
        PlaySound(dropSheepInCorrectHole);

        int randomIndex = Random.Range(0, sheepNoisesDropSheepInCorrectHole.Count);

        PlaySound(sheepNoisesDropSheepInCorrectHole[randomIndex]);
    }

    public void PlayDropSheepInWrongHoleSFX()
    {
        PlaySound(dropSheepInWrongHole);

        int randomIndex = Random.Range(0, sheepNoisesDropSheepInWrongHole.Count);

        PlaySound(sheepNoisesDropSheepInWrongHole[randomIndex]);

    }

    public void PlayHitFenceSFX()
    {
        PlaySound(hitFence);

        int randomIndex = Random.Range(0, sheepNoisesDropSheepInWrongHole.Count);

        PlaySound(sheepNoisesDropSheepInWrongHole[randomIndex]);
    }

    public void PlaySpawnWolfSFX()
    {
        int randomIndex = Random.Range(0, wolfNoisesSpawnWolf.Count);

        PlaySound(wolfNoisesSpawnWolf[randomIndex]);


    }

    public void PlayStunWolfSFX()
    {
        PlaySound(stunWolf);

        int randomIndex = Random.Range(0, wolfNoisesStunWolf.Count);

        PlaySound(wolfNoisesStunWolf[randomIndex]);
    }

    public void PlayWolfEatSheepSFX()
    {

        int randomIndex = Random.Range(0, wolfNoisesEatSheep.Count);

        PlaySound(wolfNoisesEatSheep[randomIndex]);
    }


    #endregion

    #region Play Music clips

    public void PlayTitleMusic()
    {
        PlayMusic(titleMusic);
    }
    public void PlayAmbientMusic()
    {
        //PlayMusic(ambientMusic);
    }


    #endregion
}
