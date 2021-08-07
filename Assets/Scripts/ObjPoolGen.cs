using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class ObjPoolGen : MonoBehaviour
{
    [SerializeField]
    private GameObject smallMeteoPool = null
                     , nomalMeteoPool = null
                     , bigMeteoPool = null
                     , rightDropPool = null
                     , leftDropPool = null;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetRightDrop", 0.0f, 1.0f);
        InvokeRepeating("GetLeftDrop", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 右回転の水滴を生成
    void GetRightDrop()
    {
        foreach (Transform t in rightDropPool.transform)
        {
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(InstancePoint(), Quaternion.identity);
                t.gameObject.SetActive(true);
                return;
            }
        }

        GameObject obj = rightDropPool.transform.GetChild(0).gameObject;
        Instantiate(obj, InstancePoint(), Quaternion.identity, rightDropPool.transform);
    }

    // 左回転の水滴を生成
    void GetLeftDrop()
    {
        foreach (Transform t in leftDropPool.transform)
        {
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(InstancePoint(), Quaternion.identity);
                t.gameObject.SetActive(true);
                return;
            }
        }

        GameObject obj = leftDropPool.transform.GetChild(0).gameObject;
        Instantiate(obj, InstancePoint(), Quaternion.identity, leftDropPool.transform);
    }

    // 生成位置の算出
    Vector3 InstancePoint()
    {
        Vector3 pos = new Vector3();

        // 乱数の設定
        float x = Random.Range(80.0f, 500.0f);
        float y = Random.Range(80.0f, 500.0f);
        float z = Random.Range(80.0f, 500.0f);

        bool checkX = false;
        bool checkY = false;
        bool checkZ = false;
        int xx = 0;
        int yy = 0;
        int zz = 0;
        // 正負の設定
        while (!checkX)
        {
            xx = Random.Range(-1, 2);
            if (xx != 0) checkX = true;
        }
        while (!checkY)
        {
            yy = Random.Range(-1, 2);
            if (yy != 0) checkY = true;
        }
        while (!checkZ)
        {
            zz = Random.Range(-1, 2);
            if (zz != 0) checkZ = true;
        }

        // x座標の設定
        x = x * xx;
        // y座標の設定
        y = y * yy;
        // z座標の設定
        z = z * zz;

        pos = new Vector3(x, y, z);

        return pos;
    }
}
