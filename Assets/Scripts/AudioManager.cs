using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volume=1f;
    List<AudioSource> audioSources;

    public static AudioManager singleton;
    private void Awake()
    {
        if (!singleton) singleton = this;
        else Debug.Log("Too many AudioManager");
        audioSources = new List<AudioSource>();
    }

    public AudioSource mainBGM;

    public void ChangeBGM(AudioClip clip)
    {
        mainBGM.Stop();
        mainBGM.clip = clip;
        mainBGM.Play();
    }

    public void Fadeout(float time)
    {

    }

    //IEnumerator FadingOut(AudioSource source)
    //{
    //    float 
    //}

    public void ChangeVolumeLevel(float _val)
    {
        foreach(AudioSource source in audioSources)
        {
            source.volume = _val;
        }
        volume = _val;
    }

    public void PlaySound(AudioClip _clip, float _pitch = 1f)
    {
        AudioSource freeSource = null;
        for(int i=0; i< audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                freeSource = audioSources[i];
                break;
            }
        }
        if (freeSource == null)
        {
            freeSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(freeSource);
        }
        freeSource.volume = volume;
        freeSource.pitch = _pitch;
        freeSource.PlayOneShot(_clip);
    }

}
