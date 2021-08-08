using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private int dropCount = 1;

    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private float angleSpeed = 0;

    private GameObject sunObj;

    [SerializeField]
    private float inField = 0
                , outField = 0;

    float moveZ = 0f;

    [SerializeField]
    private Slider boostSlider;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        sunObj = GameObject.FindWithTag("SunObj");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        StartPosition();
        Move();
    }

    private void FixedUpdate()
    {

        rb.velocity = transform.forward * moveZ;

        //WS�L�[�A�����L�[�ŏ㉺�̕�����ւ���
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * angleSpeed;
        transform.Rotate(Vector3.right * -y);
        //AD�L�[�A�����L�[�ŕ�����ւ���
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * angleSpeed;
        transform.Rotate(Vector3.up * x);
    }

    // ���z���痣�ꂷ������A�����ʒu�ɖ߂�
    void StartPosition()
    {
        // ���z�Ƃ̊Ԋu�𑪒�
        float distance = Vector3.Distance(sunObj.transform.position, this.transform.position);

        if (distance > 800)
        {
            this.transform.LookAt(sunObj.transform);
        }
    }

    // �ړ�
    void Move()
    {
        float scaleCorrection1 = transform.localScale.y * 0.75f;
        float scaleCorrection2 = transform.localScale.y;

        // space�L�[�őO�i
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.F) && boostSlider.value > 0)
        {
            boostSlider.value -= Time.deltaTime / 3;
            moveZ = speed * 2 * Time.deltaTime;
            if (transform.localScale.x > scaleCorrection1) transform.localScale -= new Vector3(Time.deltaTime * dropCount, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            moveZ = speed * Time.deltaTime;
            if (transform.localScale.x > scaleCorrection1) transform.localScale -= new Vector3(Time.deltaTime * dropCount, 0, 0);
        }
        else
        {
            moveZ = 0;
            boostSlider.value += Time.deltaTime / 7;
            if (transform.localScale.x <= scaleCorrection2) transform.localScale += new Vector3(Time.deltaTime * dropCount, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WaterDrop")
        {
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            boostSlider.maxValue += 0.1f;
            dropCount++;
            speed += 1.0f;
            angleSpeed += 0.1f;
        }
    }
}
