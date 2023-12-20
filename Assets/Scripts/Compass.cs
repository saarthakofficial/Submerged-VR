using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Compass : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        // GameManager.instance.compassTarget = GameManager.instance.spawnedAmuletPieces[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.compassTarget != null)
        {
            Vector3 directionToTarget = GameManager.instance.compassTarget.position - transform.position;

            transform.LookAt(transform.position + directionToTarget, Vector3.up);

            transform.localRotation = Quaternion.Euler(90,0,-transform.localEulerAngles.y);
        }


    }

    
}
