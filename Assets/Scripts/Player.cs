using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private int dropCount = 1;

    public int DropCount
    {
        get { return dropCount; }
    }

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
        if (!GameManager.instance.gameOverFlag)
        {
            StartPosition();
            Move();
            MeteoPosCheck();
        }
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

        if (distance > outField)
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

    // 覐΂ƃv���C���[�̈ʒu����A�v���C���[�̋߂���覐΂����邩����
    void MeteoPosCheck()
    {
        float distance = Vector3.Distance(FindMeteo().transform.position, this.transform.position);
        
        if (distance < 150)
        {
            Debug.Log("覐ΐڋߒ�");
            GameManager.instance.emergencyPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.emergencyPanel.SetActive(false);
        }
    }

    

    // ��ԋ߂�覐΂��擾
    GameObject FindMeteo()
    {
        GameObject[] meteos = GameObject.FindGameObjectsWithTag("Meteorite");
        GameObject closest = null;

        float distance = Mathf.Infinity;

        Vector3 position = transform.position;

        foreach (GameObject meteo in meteos)
        {
            Vector3 diff = meteo.transform.position - position;

            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = meteo;
                distance = curDistance;
            }
        }

        return closest;
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
            angleSpeed += 0.2f;
        }

        if (other.gameObject.tag == "Meteorite")
        {
            Debug.Log("�Q�[���I�[�o�[");
            GameOver();
        }
    }

    // �Q�[���I�[�o�[�̏���
    private async void GameOver()
    {
        GameManager.instance.gameOverFlag = true;

        int score = CalcScript.ScoreCalc();
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);

        await Task.Delay(1000);

    }

}
