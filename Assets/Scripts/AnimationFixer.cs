using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorFixer {
    [MenuItem("Tools/Fix Broken Animator Transitions")]
    static void FixAnimators() {
        foreach (var path in AssetDatabase.FindAssets("t:AnimatorController")) {
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(path));
            foreach (var layer in controller.layers) {
                var sm = layer.stateMachine;
                for (int i = sm.anyStateTransitions.Length - 1; i >= 0; i--) {
                    if (sm.anyStateTransitions[i].destinationState == null && sm.anyStateTransitions[i].destinationStateMachine == null)
                        sm.RemoveAnyStateTransition(sm.anyStateTransitions[i]);
                }
            }
            EditorUtility.SetDirty(controller);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Animator cleanup complete.");
    }
}