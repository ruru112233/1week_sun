using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarchScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Meteorite")
        {
            Debug.Log("障害物が近いです");
        }
    }
}
