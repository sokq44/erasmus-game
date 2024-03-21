using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLogic
{
    private static int NumberOfLevels = 3;
    private static int levelCurr = 0;

    public static void setLevelCur(int val)
    {
        levelCurr = val;
    }

    public static int getLevelCur() {  return levelCurr; }

    public static void NextLevel(bool gameOver)
    {
        if (gameOver)
        {
            SceneManager.LoadScene("GameLoseScene");
            return;
        }

        levelCurr++;

        if (levelCurr > NumberOfLevels)
            SceneManager.LoadScene("GameWinScene");
        else
            SceneManager.LoadScene("Level" + levelCurr);
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void OutLevelCurr()
    {
        UnityEngine.Debug.Log(levelCurr);
    }
}
