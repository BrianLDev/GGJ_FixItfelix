using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
	public int[] numberOfMindDemons;
	public int[] numberOfBodyDemons;
	public int[] numberOfSoulDemons;

	public GameObject levelManager;
	public GameObject audioManager;

    public Sprite DayImage;
    public Sprite NightImage;
    public Button Day_NightSwitch;

    public Camera theCamera;
    CameraShader shader;

    public static DayNightCycle instance;

    [FormerlySerializedAs("nightDuration")]
	public float defaultNightDuration = 30.0f;
	public float totalNightDuration { get; private set; }
	private float nightTimeLeft = 0.0f;

	public int currentDay = 0;

	private int randomMindDemons = 0;
	private int randomBodyDemons = 0;
	private int randomSoulDemons = 0;

	private LevelStateScript lss;
	private AudioManagerScript ams;
	private BuildingManager bm;

	// Start is called before the first frame update
	void Start()
	{
        instance = this;

        shader = theCamera.GetComponent<CameraShader>();

        lss = levelManager.GetComponent<LevelStateScript>();
		ams = audioManager.GetComponent<AudioManagerScript>();
		bm = this.transform.parent.gameObject.GetComponentInChildren<BuildingManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (nightTimeLeft > 0.0f && lss.IsPlayingGame())
		{
			nightTimeLeft -= Time.deltaTime;
			if (nightTimeLeft < 0.0f)
			{
				StartNewDay();
				return;
			}

			float productionTimeFactor = Time.deltaTime / totalNightDuration;
			int mindProduction = bm.GetMindProductionWithBonus();
			int soulProduction = bm.GetSoulProductionWithBonus();
			lss.UpdatePlayerMind(mindProduction * productionTimeFactor);
			lss.UpdatePlayerSoul(soulProduction * productionTimeFactor);
		}
	}

	public bool IsNightTime()
	{
		return nightTimeLeft > 0.0f;
	}

	public float GetNightDuration()
	{
		return totalNightDuration;
	}

	public int GetCurrentDay()
	{
		return currentDay;
	}

	public int GetNumMindDemons()
	{
		int day = GetCurrentDay();
		if (day < numberOfMindDemons.Length)
		{
			return numberOfMindDemons[day];
		}
		else
		{
			return randomMindDemons;
		}
	}

	public int GetNumBodyDemons()
	{
		int day = GetCurrentDay();
		if (day < numberOfBodyDemons.Length)
		{
			return numberOfBodyDemons[day];
		}
		else
		{
			return randomBodyDemons;
		}
	}

	public int GetNumSoulDemons()
	{
		int day = GetCurrentDay();
		if (day < numberOfSoulDemons.Length)
		{
			return numberOfSoulDemons[day];
		}
		else
		{
			return randomSoulDemons;
		}
	}

	void RandomizeDemonSpawns()
	{
		int total = Mathf.CeilToInt(Mathf.Pow(1.5f, currentDay)) - 3;
		if (total < 0)
		{
			total = 0;
		}

		randomMindDemons = 1 + Random.Range(0, total);
		total -= randomMindDemons;
		randomBodyDemons = 1 + Random.Range(0, total);
		total -= randomBodyDemons;
		randomSoulDemons = 1 + total;
	}

	public void StartNewDay()
	{
		currentDay = currentDay + 1;
		nightTimeLeft = 0.0f;
        shader.enabled = false;

        Day_NightSwitch.image.sprite = DayImage;
		ams.TransitionNightToDay();

		if (currentDay >= numberOfMindDemons.Length)
		{
			RandomizeDemonSpawns();
		}

		NightTimeListener[] spawners = FindObjectsOfType<NightTimeListener>();
		foreach (NightTimeListener listener in spawners)
		{
			listener.StartNewDay(this);
		}
	}

	public void StartNewNight(float howLong = 0.0f)
	{
		if (nightTimeLeft != 0.0f)
		{
			// Don't start a new night while there's one active!
			Debug.Assert(true);
			return;
		}

		ams.TransitionDayToNight();
        shader.enabled = true;
        Day_NightSwitch.image.sprite = NightImage;

        if (howLong == 0.0f)
		{
			howLong = defaultNightDuration;
		}
		totalNightDuration = howLong;
		nightTimeLeft = howLong;

		NightTimeListener[] spawners = FindObjectsOfType<NightTimeListener>();
		foreach (NightTimeListener listener in spawners)
		{
			listener.StartNewNight(this);
		}
	}
}
