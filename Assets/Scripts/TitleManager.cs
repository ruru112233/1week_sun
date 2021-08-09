using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button gameStartButton = null
                 , playManualButton = null;

    // Start is called before the first frame update
    void Start()
    {
        gameStartButton.onClick.SetListener(GameStartButton);
        playManualButton.onClick.SetListener(GameManualButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �Q�[���X�^�[�g
    void GameStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // �V�ѕ�
    void GameManualButton()
    {

    }
}
