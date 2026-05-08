using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/AudioSO")]
public class AudioSO : ScriptableObject
{
    public AudioClip AudioClip;
    public bool ChangePitch;
    public AudioMixerGroup mixer;
    public bool loop;

    [Range(0, 1.5f)]
    public float volume;

    [Range(0, 1.5f)]
    public float pitchRangeMin;
    [Range(0, 1.5f)]
    public float pitchRangeMax;
}
