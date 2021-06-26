using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWiledのステージデータ。
    /// 今後、ステージを増やせるようにするため
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
