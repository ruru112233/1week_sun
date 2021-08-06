using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private float angleSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // spaceキーで前進
        if (Input.GetKey(KeyCode.Space))
        {
            float z = speed * Time.deltaTime;
            transform.position += transform.forward * z;
        }
        
        //WSキー、↑↓キーで上下の方向を替える
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * angleSpeed;
        transform.Rotate(Vector3.right * y);
        //ADキー、←→キーで方向を替える
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * angleSpeed;
        transform.Rotate(Vector3.up * x);
    }
}
