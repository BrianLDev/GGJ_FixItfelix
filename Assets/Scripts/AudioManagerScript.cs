using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public bool musicCanPlay = true;

    private AudioSource[] overworldMusic;
    private AudioSource[] soundEffects;

    private AudioSource daySong;
    private AudioSource nightSong;
    private AudioSource dayToNightTrans;
    private AudioSource nightToDayTrans;
    private AudioSource demonSounds;

    private AudioSource buildingBuilt;
    private AudioSource buildingDestroyed;
    private AudioSource switchCoin;

    private float dayToNightTime;
    private float nightToDayTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        overworldMusic = mainCamera.GetComponents<AudioSource>();
        daySong = overworldMusic[0];
        nightSong = overworldMusic[1];
        dayToNightTrans = overworldMusic[2];
        nightToDayTrans = overworldMusic[3];
        demonSounds = overworldMusic[4];

        soundEffects = this.GetComponents<AudioSource>();
        buildingBuilt = soundEffects[0];
        buildingDestroyed = soundEffects[1];
        switchCoin = soundEffects[2];

        dayToNightTime = 0.0f;
        nightToDayTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (musicCanPlay)
        {
            if (dayToNightTime > 0.0f)
            {
                nightSong.volume += 0.1f * Time.deltaTime;
                daySong.volume -= 0.4f * Time.deltaTime;
                dayToNightTime -= Time.deltaTime;
                if (dayToNightTime < 0.0f)
                {
                    daySong.Stop();
                    dayToNightTrans.Stop();
                    nightSong.volume = 1.0f;
                    demonSounds.Play();
                    demonSounds.volume = 0.3f;
                }
            }

            if (nightToDayTime > 0.0f)
            {
                daySong.volume += 0.1f * Time.deltaTime;
                nightSong.volume -= 0.4f * Time.deltaTime;
                nightToDayTime -= Time.deltaTime;
                if (nightToDayTime < 0.0f)
                {
                    nightSong.Stop();
                    nightToDayTrans.Stop();
                    daySong.volume = 1.0f;
                }
            }
        }
    }

    public void StopAllMusic()
    {
        foreach (AudioSource aSrc in overworldMusic)
        {
            aSrc.Stop();
        }
    }

    public void TransitionDayToNight()
    {
        dayToNightTime = 6.0f;
        nightSong.Play();
        nightSong.volume = 0.0f;
        dayToNightTrans.Play();
    }

    public void TransitionNightToDay()
    {
        nightToDayTime = 6.0f;
        daySong.Play();
        daySong.volume = 0.0f;
        nightToDayTrans.Play();
        demonSounds.Stop();
    }

    public void PlayBuildingBuilt()
    {
        buildingBuilt.Play();
    }

    public void PlayBuildingDestroyed()
    {
        buildingDestroyed.Play();
    }

    public void PlaySwitchCoin()
    {
        switchCoin.Play();
    }
}
