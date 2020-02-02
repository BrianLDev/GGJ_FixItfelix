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
    private AudioSource dayAmbiance;

    private AudioSource buildingBuilt;
    private AudioSource buildingDestroyed;
    private AudioSource switchCoin;
    private AudioSource upgradeLibrary;
    private AudioSource upgradeGym;
    private AudioSource upgradeVice;
    private AudioSource upgradeAmp;
    private AudioSource upgradeMarket;

    private AudioSource angelTheme;
    private AudioSource steelTheme;
    private AudioSource jacqueTheme;
    private AudioSource lysTheme;

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
        dayAmbiance = overworldMusic[5];

        soundEffects = this.GetComponents<AudioSource>();
        buildingBuilt = soundEffects[0];
        buildingDestroyed = soundEffects[1];
        switchCoin = soundEffects[2];
        upgradeLibrary = soundEffects[3];
        upgradeMarket = soundEffects[4];
        upgradeAmp = soundEffects[5];
        upgradeGym = soundEffects[6];
        upgradeVice = soundEffects[7];

        angelTheme = soundEffects[8];
        steelTheme = soundEffects[9];
        jacqueTheme = soundEffects[10];
        lysTheme = soundEffects[11];

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
                    dayAmbiance.Play();
                    dayAmbiance.volume = 0.3f;
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
        dayAmbiance.Stop();
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

    public void PlayUpLibrary()
    {
        upgradeLibrary.Play();
    }

    public void PlayUpMarket()
    {
        upgradeMarket.Play();
    }

    public void PlayUpAmp()
    {
        upgradeAmp.Play();
    }

    public void PlayUpGym()
    {
        upgradeAmp.Play();
    }

    public void PlayUpVice()
    {
        upgradeVice.Play();
    }

    public void PlayAngelTheme()
    {
        angelTheme.Play();
    }

    public void PlaySteelTheme()
    {
        steelTheme.Play();
    }

    public void PlayJacqueTheme()
    {
        jacqueTheme.Play();
    }

    public void PlayLysTheme()
    {
        lysTheme.Play();
    }
}
