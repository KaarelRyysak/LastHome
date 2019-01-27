using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/AudioGroup")]
public class AudioGroup : ScriptableObject
{

    [Range(0, 2)]
    public float VolumeMin = 1f;
    [Range(0, 2)]
    public float VolumeMax = 1f;
    [Range(0, 2)]
    public float PitchMin = 1f;
    [Range(0, 2)]
    public float PitchMax = 1f;


    public float Cooldown = 0.1f;
    public AudioClip[] audioClips;

    private float timeStamp;
    private AudioSourcePool audioSourcePool;

    private void OnEnable()
    {
        timeStamp = -Cooldown;
        audioSourcePool = FindObjectOfType<AudioSourcePool>();
    }


    public void Play()
    {
        if (!audioSourcePool)
        {
            audioSourcePool = FindObjectOfType<AudioSourcePool>();
        }

        AudioSource source = audioSourcePool.GetSource();
        source.transform.SetParent(null);
        source.transform.position = Vector3.zero;
        source.spatialBlend = 0;
        Play(source);
    }


    public void Play(Vector3 location)
    {
        if (!audioSourcePool)
        {
            audioSourcePool = FindObjectOfType<AudioSourcePool>();
        }
        AudioSource source = audioSourcePool.GetSource();
        source.transform.position = location;
        source.transform.SetParent(null);
        source.spatialBlend = 1;
        Play(source);
    }

    public void Play(Transform transform)
    {
        if (!audioSourcePool)
        {
            audioSourcePool = FindObjectOfType<AudioSourcePool>();
        }
        AudioSource source = audioSourcePool.GetSource();
        source.transform.position = Vector3.zero;
        source.transform.SetParent(transform);
        source.spatialBlend = 1;

        Play(source);
    }


    public void Play(AudioSource source)
    {
        if (audioClips.Length == 0) return;
        //timestamp check
        if (Time.time < timeStamp + Cooldown) return;

        timeStamp = Time.time;

        source.clip = audioClips[Random.Range(0, audioClips.Length)];
        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);

        source.Play();



    }


}
