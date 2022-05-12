using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CChangeScenes
{
    public enum EOtherScenes
    {
        eBoot             = 0,
        eHistorvWindow    = 1,
        eMax,
    };


    public void ChangeScenes(string lScenesName)
    {
        //SceneManager.LoadScene(lScenesName);

        //GlobalData.g_CurSceneName = lScenesName;

        //string[] sArray = lScenesName.Split(new string[] { GlobalData.g_scLevelPrefix }, StringSplitOptions.RemoveEmptyEntries);

        //if (sArray.Length == 1)
        //    GlobalData.g_LevelIndex = int.Parse(sArray[0]);
    }

    public void LoadGameScenes(int LevelIndex = -1)
    {
        int lTempIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (LevelIndex == -1)
        {
            if (lTempIndex >= SceneManager.sceneCountInBuildSettings)
                lTempIndex = 1;
        }
        else
            lTempIndex = LevelIndex;

        StaticGlobalDel.g_CurSceneName = StaticGlobalDel.g_GameScenesName;
        CSaveManager.m_status.LevelIndex.Value = lTempIndex;
        SceneManager.LoadScene(lTempIndex);
    }

    public void LoadTestScenes()
    {
        StaticGlobalDel.g_CurSceneName = StaticGlobalDel.g_testScenesName;
        SceneManager.LoadScene(StaticGlobalDel.g_CurSceneName);
    }

    public void SetNextLevel()
    {
        int lTempNextLevelIndex = CSaveManager.m_status.m_LevelIndex;
        lTempNextLevelIndex++;

        //if (lTempNextLevelIndex >= GlobalData.SharedInstance.LevelGameObj.Length)
        //    lTempNextLevelIndex = 0;

        CSaveManager.m_status.LevelIndex.Value = lTempNextLevelIndex;
       // CSaveManager.Save();
    }


    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       // CSaveManager.Save();
    }

    public void AdditiveLoadScenes(String Scenestext, Action onSceneLoadedOK = null)
    {
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (onSceneLoadedOK != null)
                onSceneLoadedOK();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(Scenestext, LoadSceneMode.Additive);
    }

    public void RemoveScenes(String Scenestext)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (Scenestext == SceneManager.GetSceneAt(i).name && SceneManager.GetSceneAt(i).isLoaded)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
        }
    }

    //public int NameToIndex(string lpScenesName)
    //{
    //    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
    //    {
    //        if ( SceneManager.GetSceneByBuildIndex(i).name == lpScenesName)
    //            return i;
    //    }

    //    return -1;
    //}
}
