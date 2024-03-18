using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    Button playBtn;
    Button exitBtn;
    AudioSource menuMusic;

    void Start()
    {
        playBtn = GameObject.Find("PlayButton").GetComponent<Button>();
        exitBtn = GameObject.Find("ExitButton").GetComponent<Button>();
        menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();

        ScenesLogic.setLevelCur(0);

        playBtn.onClick.AddListener(() =>
        {
            menuMusic.Stop();
            ScenesLogic.NextLevel(false);
        });

        exitBtn.onClick.AddListener(() => EditorApplication.isPlaying = false );
    }    
}
