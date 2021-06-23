using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;//OnOpenAsset���g���̂ɕK�v
using UnityEditor;
using System.IO;
using System.Diagnostics;


namespace Musahi.MyTools
{
    /// <summary>
    /// ����̃A�v���P�[�V�������AUnity����J����悤�ɂ���N���X
    /// </summary>
    public class SpecificOpenApp : Editor
    {
        /// <summary>
        /// .shader�A.hlsl�t�@�C����vscode�ŊJ��(�����p)
        /// </summary>
        public static class OpneVSCode
        {
            private const string SHADERFile = ".shader";
            private const string HLSLFile = ".hlsl";

            [OnOpenAsset(0)]
            public static bool OnOpen(int instanceID, int line)
            {
                //�J�����A�Z�b�g���擾
                string path = AssetDatabase.GetAssetPath(instanceID);

                //�g���q��.shader�܂��́Ahlsl�łȂ��Ȃ炻�̂܂܃t�@�C�����J��
                if (Path.GetExtension(path) != SHADERFile && Path.GetExtension(path) != HLSLFile) return false;

                string fullPath = Path.GetFullPath(path);

                Process.Start("code", fullPath);
                return true;
            }
        }

        public class OpenPhotoshop : Editor
        {
            const string ItemName = "Assets/Open in Photoshop";

            [MenuItem(ItemName, false)]
            static void Opne()
            {
                foreach (var guid in Selection.assetGUIDs)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var fullPath = Path.GetFullPath(path);
                    Process.Start("photoshop", fullPath);
                }
            }
        }
    }
}