using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public GlobalSoundManager globalSoundManager;
	public AudioClip roundBGM;
	public AudioClip gameOverBGM;
	public GameObject inspectorPrefab;
	public GameObject indicatorUIPrefab;
	public AudioClip deathSound;
	public GameObject deadCam;
	public GameObject deadScreen;

	CharacterController controller;
	HealthManager healthManager;
	Transform character;
	Transform rootRig;
	AudioSource audioSource;
	bool isDestroyed = false;

	public int upgradeHealth = 0;
	public int upgradeRegeneration = 0;

	void Awake() {
		controller = GetComponent<CharacterController>();
		healthManager = GetComponent<HealthManager>();
		character = transform.Find("Character");
		rootRig = character.Find("Healthmale/Root");
	}
	
	void Start() {
		DeactivateRagdoll();

		globalSoundManager.PlayMusic(roundBGM);
	}

	void Update() {
		if(healthManager.IsDead && !isDestroyed) {
			isDestroyed = true;

			Transform fpsChar = transform.Find("FirstPersonCharacter");
				
			fpsChar.gameObject.SetActive(false);
			character.gameObject.SetActive(true);
			character.GetComponent<SoundManager>().Play(deathSound);
			character.SetParent(null);
			ActivateRagdoll();
			
			GameObject.Find("UI/InGameUI/PlayerUI").SetActive(false);
			deadCam.SetActive(true);

			GameOver();
		}
	}

	void GameOver() {
		globalSoundManager.PlayMusic(gameOverBGM);

		deadScreen.SetActive(true);
		StartCoroutine(ShowDeadScreenAndCleanup());
	}

	IEnumerator ShowDeadScreenAndCleanup() {
		deadScreen.SetActive(true);

		Image image = deadScreen.GetComponent<Image>();
		Color origColor = image.color;

		for(float alpha = 0.0f; alpha <= 1.1f; alpha += 0.1f) {
			image.color = new Color(origColor.r, origColor.g, origColor.b, alpha);
			yield return new WaitForSeconds(0.1f);
		}
		
		yield return new WaitForSeconds(10f);		// Just wait 10 seconds more to reset
		SceneManager.LoadScene("Map1");
	}

	void ActivateRagdoll() {
		controller.enabled = false;
		character.GetComponent<Animator>().enabled = false;

		// Hide weapons
		character.Find("Glock").GetComponent<SkinnedMeshRenderer>().enabled = false;
		character.Find("Colt Python").GetComponent<SkinnedMeshRenderer>().enabled = false;
		character.Find("MP5K").GetComponent<SkinnedMeshRenderer>().enabled = false;
		character.Find("UMP45").GetComponent<SkinnedMeshRenderer>().enabled = false;
		character.Find("M870").GetComponent<SkinnedMeshRenderer>().enabled = false;
		character.Find("AKM").GetComponent<SkinnedMeshRenderer>().enabled = false;

		CharacterJoint[] joints = rootRig.GetComponentsInChildren<CharacterJoint>();
		Rigidbody[] rigidbodies = rootRig.GetComponentsInChildren<Rigidbody>();
		Collider[] colliders = rootRig.GetComponentsInChildren<Collider>();
		
		foreach(CharacterJoint joint in joints) {
			joint.enablePreprocessing = false;
			joint.enableProjection = true;
		}
		foreach(Rigidbody rigidbody in rigidbodies) {
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
		}
		foreach(Collider collider in colliders) {
			collider.enabled = true;
		}
		
	}

	void DeactivateRagdoll() {
		Rigidbody[] rigidbodies = rootRig.GetComponentsInChildren<Rigidbody>();
		Collider[] colliders = rootRig.GetComponentsInChildren<Collider>();

		foreach(Rigidbody rigidbody in rigidbodies) {
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}
		foreach(Collider collider in colliders) {
			collider.enabled = false;
		}
	}

	void DisableWeapon(WeaponBase weapon) {
		weapon.IsEnabled = false;
	}

	void DisableController(FirstPersonController controller) {
		controller.enabled = false;
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.gameObject.tag == "BulletCase") {
			Physics.IgnoreCollision(GetComponent<Collider>(), hit.gameObject.GetComponent<Collider>());
		}
	}

	public void ActivateHealthRegeneration() {
		StartCoroutine(CoHealthRegeneration());
	}

	IEnumerator CoHealthRegeneration() {
		while(!healthManager.IsDead) {
			float nextDelay = 5f - (upgradeRegeneration * 0.35f);
			
			if(healthManager.Health < healthManager.MaxHealth) {
				healthManager.SetHealth(healthManager.Health + 1);
			}

			yield return new WaitForSeconds(nextDelay);
		}

		yield break;
	}
}
