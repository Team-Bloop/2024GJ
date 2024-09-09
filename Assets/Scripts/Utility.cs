using UnityEngine;

namespace GeneralUtility
{
    public class Utility
    {
        public static void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif 
            
            Application.Quit();
        }
    }
}