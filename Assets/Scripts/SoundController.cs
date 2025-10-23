using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip loopSound;
	private float startTime;
	private GameObject HUD;

	// Start is called before the first frame update
	void Start()
	{
		HUD = GameObject.FindGameObjectWithTag("HUD");
		StartCoroutine(Waiting());
	}
	
	IEnumerator Waiting()
	{
		while (HUD.GetComponent<UIManager>().countDownDone == false)
		{
			yield return null;
		}
		audioSource.clip = loopSound;
		audioSource.Play();
		audioSource.loop = true;
    }
}
