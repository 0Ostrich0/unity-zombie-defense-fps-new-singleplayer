using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	public AudioClip attackSound;
	public AudioClip deathSound;
	public Material deadMaterial;
	private Animator animator;
	private AudioSource audioSource;
	private Chasing chasing;
	private HealthManager healthManager;
	private bool wasAlreadyDead = false;

	void Start() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		chasing = GetComponent<Chasing>();
		healthManager = GetComponent<HealthManager>();
	}

	void Update() {
		if(wasAlreadyDead == true) return;

		// Stop chasing and set dead if it's dead
		if(!wasAlreadyDead && healthManager.IsDead) {
			wasAlreadyDead = true;

			SetDead();
			chasing.StopChasing();
		}
	}
	
	void SetDead() {
		RemoveColliders(GetComponents<Collider>());
		RemoveColliders(GetComponentsInChildren<Collider>());

		animator.SetTrigger("Dead");
		audioSource.PlayOneShot(deathSound);

		transform.Find("Icon").GetComponent<MeshRenderer>().material = deadMaterial;
		
		Destroy(gameObject, 5f);
	}

	void RemoveColliders(Collider[] colliders) {
		foreach(Collider collider in colliders) {
			collider.enabled = false;
		}
	}

	public void PlayAttackAnimation() {
		animator.SetTrigger("Attack");
		audioSource.PlayOneShot(attackSound);
	}
}
