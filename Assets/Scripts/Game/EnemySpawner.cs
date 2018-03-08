using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
	[Header("Enemy Spawn Management")]
	public GameObject zombiePrefab;
	public int maxZombies = 20;
	public float respawnDuration = 10.0f;
	public List<GameObject> spawnPoints = new List<GameObject>();
	
	[Header("Enemy Status")]
	public float startHealth = 100f;
	public float startMoveSpeed = 1f;
	public float startDamage = 15f;
	public int startEXP = 3;
	public int startFund = 4;
	public float upgradeDuration = 60f;	// Increase all enemy stats every 30 seconds

	private float upgradeTimer;
	[SerializeField] private float currentHealth;
	[SerializeField] private float currentMoveSpeed;
	[SerializeField] private float currentDamage;
	[SerializeField] private int currentEXP;
	[SerializeField] private int currentFund;

	private bool activate = true;
	private float spawnTimer;


	void Start() {
		currentHealth = startHealth;
		currentMoveSpeed = startMoveSpeed;
		currentDamage = startDamage;
		currentEXP = startEXP;
		currentFund = startFund;

		spawnTimer = respawnDuration;	// Spawns instantly
	}

	void Update() {
		if(!activate) return;

		if(spawnTimer < respawnDuration) {
			spawnTimer += Time.deltaTime;
		}
		else {
			SpawnEnemy();
		}

		if(upgradeTimer < upgradeDuration) {
			upgradeTimer += Time.deltaTime;
		}
		else {
			UpgradeEnemy();
		}
	}

	void SpawnEnemy() {
		if(spawnTimer < respawnDuration) return;

		int spawnCount = 0;
		int maxSpawnCount = 5;
		int zombiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

		foreach(GameObject spawnPoint in spawnPoints) {
			// If zombies were spawned too many, just stop.
			if(zombiesCount >= maxZombies) break;

			// Check how many zombies are spawning once by player numbers
			else if(spawnCount >= maxSpawnCount) break;

			GameObject zombie = Instantiate(zombiePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

			zombie.GetComponent<HealthManager>().SetHealth(currentHealth);

			KillReward killReward = zombie.GetComponent<KillReward>();
			killReward.SetReward(currentEXP, currentFund);

			// Boost rotating speed
			float rotateSpeed = 120f + currentMoveSpeed;
			rotateSpeed = Mathf.Max(rotateSpeed, 200f);	// Max 200f

			Chasing chasing = zombie.GetComponent<Chasing>();
			chasing.SetDamage(currentDamage);
			chasing.SetSpeed(currentMoveSpeed, rotateSpeed);

			spawnCount++;
			zombiesCount++;
		}
		
		spawnTimer = 0f;
	}

	void UpgradeEnemy() {
		currentHealth += 5;

		if(currentMoveSpeed < 6.0f) {
			currentMoveSpeed += 0.4f;
		}
		if(currentDamage < 90f) {
			currentDamage += 2f;
		}
		else {
			currentDamage = 90;
		}
		
		currentEXP++;
		currentFund++;

		upgradeTimer = 0;
	}
}
