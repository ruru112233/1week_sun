using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSearch : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        LookAtDrop();
    }

    // 一番近くにある水滴の向きに向く
    void LookAtDrop()
    {
        GameObject obj = SearchScript.FindDrop(player.transform);
        if (obj != null)
        {
            this.transform.LookAt(obj.transform);
        }
        
    }
}
