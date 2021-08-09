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

        //WSキー、↑↓キーで上下の方向を替える
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * angleSpeed;
        transform.Rotate(Vector3.right * -y);
        //ADキー、←→キーで方向を替える
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * angleSpeed;
        transform.Rotate(Vector3.up * x);
    }

    // 太陽から離れすぎたら、初期位置に戻す
    void StartPosition()
    {
        // 太陽との間隔を測定
        float distance = Vector3.Distance(sunObj.transform.position, this.transform.position);

        if (distance > outField)
        {
            this.transform.LookAt(sunObj.transform);
        }
    }

    // 移動
    void Move()
    {
        float scaleCorrection1 = transform.localScale.y * 0.75f;
        float scaleCorrection2 = transform.localScale.y;

        // spaceキーで前進
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

    // 隕石とプレイヤーの位置から、プレイヤーの近くに隕石があるか判定
    void MeteoPosCheck()
    {
        float distance = Vector3.Distance(FindMeteo().transform.position, this.transform.position);
        
        if (distance < 150)
        {
            Debug.Log("隕石接近中");
            GameManager.instance.emergencyPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.emergencyPanel.SetActive(false);
        }
    }

    

    // 一番近い隕石を取得
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
            Debug.Log("ゲームオーバー");
            GameOver();
        }
    }

    // ゲームオーバーの処理
    private async void GameOver()
    {
        GameManager.instance.gameOverFlag = true;

        int score = CalcScript.ScoreCalc();
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);

        await Task.Delay(1000);

    }

}
