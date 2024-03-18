using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour
{
    public string scene;
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => SceneManager.LoadScene(scene) );
    }
}
