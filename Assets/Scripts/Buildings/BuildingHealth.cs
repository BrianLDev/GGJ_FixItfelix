using UnityEngine;

[RequireComponent(typeof(BuildingInfo))]
public class BuildingHealth : MonoBehaviour
{
	[HideInInspector]
	public BuildingManager BuildingManager;

	public int[] BaseHealthData;

	public int MaxHealth { get; private set; }
	public int CurrentHealth { get; private set; }

	private int _healthLevel;

	private void Start()
	{
		_healthLevel = 0;
		MaxHealth = BaseHealthData[_healthLevel]; // TODO query GameManager for health multiplier
		CurrentHealth = MaxHealth;
	}

	public void DealDamage(int amount)
	{
		CurrentHealth -= amount;
		if (CurrentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}

	public bool CanUpgradeHealth()
	{
		return _healthLevel + 1 < BaseHealthData.Length;
	}

	public void DoUpgradeHealth()
	{
		_healthLevel += 1;
		MaxHealth = BaseHealthData[_healthLevel]; // TODO query GameManager for health multiplier
		// TODO Update current health
	}

	public void OnHealthMultiplierChanged(int prevPercent, int nextPercent)
	{
		// TODO
	}
}
