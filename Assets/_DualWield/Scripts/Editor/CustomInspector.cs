using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    ///�C���X�y�N�^�[��ҏW����N���X���܂Ƃ߂��N���X. �����ɁuCI�v�ƕt����
    /// </summary>
    public class CustomInspector 
    {
        [CustomEditor(typeof(SelectStageTrigger))]
        public class SelectStageTriggerCI : Editor
        {
            SelectStageTrigger stageTrigger;

            private void OnEnable()
            {
                stageTrigger = target as SelectStageTrigger;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (!stageTrigger) return;
           
                if (EditorApplication.isPlaying && GUILayout.Button("Invoke"))
                {
                    stageTrigger.OnDamage();
                }
            }
        }
    }
}
