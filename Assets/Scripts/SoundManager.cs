﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public void Play(AudioClip clip) {
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.clip = clip;
		audioSource.minDistance = 5f;
		audioSource.maxDistance = 100f;
		audioSource.spatialBlend = 1f;
		audioSource.Play();

		StartCoroutine(RemoveSoundComponent(audioSource));
	}

	IEnumerator RemoveSoundComponent(AudioSource audioSource) {
		yield return new WaitForSeconds(audioSource.clip.length);
		Destroy(audioSource);
	}
}
