using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{

    [SerializeField] GameObject audioPrefab;

    [Header("SFX Clips")]
    [SerializeField] AudioSO hoverOnSheep;
    [SerializeField] AudioSO grabSheep;
    [SerializeField] AudioSO hoverOnHole;
    [SerializeField] AudioSO dropSheepInCorrectHole;
    [SerializeField] AudioSO dropSheepInWrongHole;
    [SerializeField] AudioSO hitFence;
    [SerializeField] AudioSO stunWolf;
    [SerializeField] AudioSO wolfEatSheep;




    [Header("Music Clips")]
    [SerializeField] AudioSO titleMusic;



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
    }

    public void PlayGrabSheepSFX(bool status)
    {
        if(status)
        {
            PlaySound(grabSheep);
        }
        
    }
    public void PlayHoverOnHoleSFX()
    {
        PlaySound(hoverOnHole);
    }

    public void PlayDropSheepInCorrectHoleSFX()
    {
        PlaySound(dropSheepInCorrectHole);
    }

    public void PlayDropSheepInWrongHoleSFX()
    {
        PlaySound(dropSheepInWrongHole);
    }

    public void PlayHitFenceSFX()
    {
        PlaySound(hitFence);
    }

    public void PlayStunWolfSFX()
    {
        PlaySound(stunWolf);
    }

    public void PlayWolfEatSheepSFX()
    {
        PlaySound(wolfEatSheep);
    }


    #endregion

    #region Play Music clips

    public void PlayTitleMusic()
    {
        PlayMusic(titleMusic);
    }



    #endregion
}
