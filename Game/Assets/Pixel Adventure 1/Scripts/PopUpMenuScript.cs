using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUpMenuScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button exitBtn;

    [SerializeField] private bool popUpActive = false;

    private void Start()
    {
        image = GetComponent<Image>();
        mainMenuBtn = GameObject.Find("MainMenuBtn").GetComponent<Button>();
        retryBtn = GameObject.Find("RetryBtn").GetComponent<Button>();
        exitBtn = GameObject.Find("ExitBtn").GetComponent<Button>();

        mainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("GameMenu"));
        retryBtn.onClick.AddListener(() => SceneManager.LoadScene("Level" + ScenesLogic.getLevelCur()));
        exitBtn.onClick.AddListener(() => Application.Quit() );

        ChangeVisibility(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ChangeVisibility(!popUpActive);
    }

    void ChangeVisibility(bool p)
    {
        popUpActive = p;
        image.enabled = p;

        foreach (Transform child in image.transform)
            child.gameObject.SetActive(p);  
    }
}
