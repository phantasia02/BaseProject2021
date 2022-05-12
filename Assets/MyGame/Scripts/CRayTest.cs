using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRayTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray lTempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
       

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(lTempRay, out hit, Mathf.Infinity, 128))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
    }
}
