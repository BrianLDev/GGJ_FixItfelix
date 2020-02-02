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
		MaxHealth = BaseHealthData[_healthLevel] * (100 + BuildingManager.CachedHealthBonusPercent) / 100;
		CurrentHealth = MaxHealth;
	}

	public void DealDamage(int amount)
	{
		CurrentHealth -= amount;
		CheckForDestruction();
	}

	private void CheckForDestruction()
	{
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
		int oldMaxHealth = MaxHealth;
		MaxHealth = BaseHealthData[_healthLevel] * (100 + BuildingManager.CachedHealthBonusPercent) / 100;
		CurrentHealth += MaxHealth - oldMaxHealth;
	}

	public void DoRepair()
	{
		CurrentHealth = MaxHealth;
	}

	public void OnHealthBonusChanged(int prevPercent, int nextPercent)
	{
		int oldMaxHealth = MaxHealth;
		MaxHealth = BaseHealthData[_healthLevel] * (100 + nextPercent) / 100;
		CurrentHealth += MaxHealth - oldMaxHealth;
		CheckForDestruction();
	}
}
