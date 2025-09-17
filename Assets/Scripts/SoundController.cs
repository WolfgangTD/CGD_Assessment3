using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip loopSound;
	private float startTime;
	
	// Start is called before the first frame update
	void Start()
	{
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time - startTime > 3f && audioSource.clip != loopSound)
		{
			audioSource.Stop();
			audioSource.clip = loopSound;

			audioSource.Play();
			audioSource.loop = true;
        }
	}
}
