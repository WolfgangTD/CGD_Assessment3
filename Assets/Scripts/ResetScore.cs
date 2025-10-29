using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class ResetScore {
    [MenuItem("Tools/ResetScore")]
    static void resetScore() {

        PlayerPrefs.SetInt("HIGHSCORE", 0);
        PlayerPrefs.SetFloat("HIGHSCORE_TIME", 0);
        
    }
}