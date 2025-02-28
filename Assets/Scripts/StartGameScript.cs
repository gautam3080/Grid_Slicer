using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartGameScript : MonoBehaviour
{
    [SerializeField] GameObject _startPanel;
    [SerializeField] Button _startButton;
    [SerializeField] Button _exitButton;
    void Start()
    {
        AddListeners();
    }

    void AddListeners()
    {
        _startButton.onClick.AddListener(StartGameFun);
        _exitButton.onClick.AddListener(EndGameFun);

    }

    void StartGameFun()
    {
        SceneManager.LoadScene("GamePlayScene", LoadSceneMode.Additive);
        _startPanel.SetActive(false);
    }

     void EndGameFun()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
