using UnityEngine;

[RequireComponent(typeof(BuildingInfo))]
public class BuildingHealth : MonoBehaviour
{
	[SerializeField] GameObject FirePrefab;

	[HideInInspector]
	public BuildingManager BuildingManager;
	public int[] BaseHealthData;

	public Sprite WallSprite;

	public int MaxHealth { get; private set; }
	public int CurrentHealth { get; private set; }

	public int HealthUpgradeLevel { get; private set; }

	private void Start()
	{
		HealthUpgradeLevel = 0;
		MaxHealth = BaseHealthData[HealthUpgradeLevel] * (100 + BuildingManager.CachedHealthBonusPercent) / 100;
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
			GameObject fire = Instantiate(FirePrefab, this.transform.position, Quaternion.identity);
			Destroy(fire, 3f);
			Destroy(gameObject);
		}
	}

	public bool CanUpgradeHealth()
	{
		return HealthUpgradeLevel + 1 < BaseHealthData.Length;
	}

	public void DoUpgradeHealth()
	{
		HealthUpgradeLevel += 1;
		int oldMaxHealth = MaxHealth;
		MaxHealth = BaseHealthData[HealthUpgradeLevel] * (100 + BuildingManager.CachedHealthBonusPercent) / 100;
		CurrentHealth += MaxHealth - oldMaxHealth;
	}

	public void DoRepair()
	{
		CurrentHealth = MaxHealth;
	}

	public void OnHealthBonusChanged(int prevPercent, int nextPercent)
	{
		int oldMaxHealth = MaxHealth;
		MaxHealth = BaseHealthData[HealthUpgradeLevel] * (100 + nextPercent) / 100;
		CurrentHealth += MaxHealth - oldMaxHealth;
		CheckForDestruction();
	}
}
