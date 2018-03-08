using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {
	[SerializeField] private float health = 100.0f;
	[SerializeField] private float maxHealth = 100.0f;
	public HealthManager referer;	// Special prorperty for create multiple hit system, if it sets on GameObject that has same HealthManager, Apply Damage to it.
	public float damageFactor = 1.0f;

	void Start() {
		maxHealth = health;
	}

	public float Health {
		get {
			return health;
		}
	}
	
	public float MaxHealth {
		get {
			return maxHealth;
		}
	}

	public void ApplyDamage(float damage) {
		if(IsDead) return;

		damage *= damageFactor;

		if(referer) {
			referer.ApplyDamage(damage);
		}
		else {
			health -= damage;

			if(health <= 0) {
				health = 0;
			}
		}
	}

	public void SetHealth(float newHealth) {
		health = newHealth;
	}

	public void SetMaxHealth(float newHealth) {
		maxHealth = newHealth;
	}

	public void SetDamageFactor(float newFactor) {
		damageFactor = newFactor;
	}

	public void Heal() {
		SetHealth(maxHealth);
	}

	public bool IsDead {
		get {
			if(!referer) {
				return health <= 0;
			}
			else {
				return referer.IsDead;
			}
		}
	}
}
