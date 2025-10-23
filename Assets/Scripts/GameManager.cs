using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("GameManager"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
