using UnityEngine;
using UnityEditor;
using System.Linq;

namespace com.F4A.MobileThird
{
    [CustomEditor(typeof(AudioManager))]
    [CanEditMultipleObjects]
    public class AudioEditor : Editor
    {
        AudioManager audioManager;

        private void OnEnable()
        {
            audioManager = (AudioManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Setup Audio Source"))
            {
                audioManager.SetupAudioSource();
            }
        }
    }
}