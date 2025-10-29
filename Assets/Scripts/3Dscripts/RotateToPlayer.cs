using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FaceCamera();
    }
    void FaceCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 direction = transform.position - cam.transform.position;
        direction.y = 0f; 

        if (direction.sqrMagnitude > 0.0001f) 
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
