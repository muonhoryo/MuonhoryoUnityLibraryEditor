
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace MuonhoryoLibrary.Unity.Editor
{
    [CustomEditor(typeof(OnStartInactiveObject))]
    internal sealed class OnStartInactiveObject_Editor : UnityEditor.Editor
    {
        private MonoBehaviour owner;

        private void CheckScriptsWithStartMethod()
        {
            foreach (var script in owner.GetComponents<MonoBehaviour>())
            {
                if (script == owner)
                    continue;
                var methInfo = script.GetType().GetMethod("Start", System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);
                if (methInfo != null && methInfo.GetParameters().Length == 0)
                {
                    UnityEngine.Debug.LogError($"Object {owner.gameObject} has script with implemented Start() method." +
                        $"OnStartInactiveObject has been destroyed for correct working of scripts.");
                    DestroyImmediate(owner);
                }
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            owner = target as MonoBehaviour;
            CheckScriptsWithStartMethod();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            CheckScriptsWithStartMethod();
            base.OnInspectorGUI();
        }
    }
}
