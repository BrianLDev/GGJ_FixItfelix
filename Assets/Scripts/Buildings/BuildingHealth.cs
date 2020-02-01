using UnityEngine;

[RequireComponent(typeof(BuildingInfo))]
public class BuildingHealth : MonoBehaviour
{
	public int MaxHealth { get; private set; }
	public int CurrentHealth { get; private set; }

	private void Start()
	{
		BuildingInfo info = GetComponent<BuildingInfo>();

		MaxHealth = info.BaseMaxHealth; // TODO query GameManager for health multiplier
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
}
