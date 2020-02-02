using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    private AudioSource daySong;
    private AudioSource nightSong;
    private AudioSource dayToNightTrans;
    private AudioSource nightToDayTrans;

    private float dayToNightTime;
    private float nightToDayTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        AudioSource[] overworldMusic = mainCamera.GetComponents<AudioSource>();
        daySong = overworldMusic[0];
        nightSong = overworldMusic[1];
        dayToNightTrans = overworldMusic[2];
        nightToDayTrans = overworldMusic[3];

        dayToNightTime = 0.0f;
        nightToDayTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
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
    }
}
