using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Options : MonoBehaviour
{
    public void SaveAndQuit()
    {
        SaveManager.instance.SaveGame();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
