using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWiled�̃X�e�[�W�f�[�^�B
    /// ����A�X�e�[�W�𑝂₹��悤�ɂ��邽��
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "LevelData", menuName = "DualWield/LevelSettingData")]
    public class LevelSettingSOData : ScriptableObject
    {
        [SerializeField] int levelID;
        [SerializeField] GameObject levelPrefab;
    
        public int GetID => levelID;
        public GameObject GetLevelPrefab => levelPrefab;
    }
}
