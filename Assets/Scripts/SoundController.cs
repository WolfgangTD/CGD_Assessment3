using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip loopSound;
	public AudioClip scaredSound;
	public AudioClip deadSound;
	private float startTime;
	private GameObject HUD;

	// Start is called before the first frame update
	void Start()
	{
		HUD = GameObject.FindGameObjectWithTag("HUD");
		StartCoroutine(Waiting());
	}
	public void ScaredState()
    {
        audioSource.clip = scaredSound;
		audioSource.Play();
		audioSource.loop = true;
    }
	public void DeadState()
    {
        audioSource.clip = deadSound;
		audioSource.Play();
		audioSource.loop = true;
    }

	public void BackToNormal()
    {
        audioSource.clip = loopSound;
		audioSource.Play();
		audioSource.loop = true;
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
