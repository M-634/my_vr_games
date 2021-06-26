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
        [SerializeField] PlayableDirector director;

        public int ID => levelID;
        public GameObject Level => levelPrefab;
        public PlayableDirector GetDirector => director;

        /// <summary>
        /// ステージオブジェクト全体のアクティブを切り替える関数
        /// </summary>
        /// <param name="value"></param>
        public void ActiveLevel(bool value)
        {
            if (levelPrefab)
            {
                levelPrefab.SetActive(value);
            }
        }

        /// <summary>
        /// ステージのタイムラインを再生関数
        /// </summary>
        public void PlayLevelDirector()
        {
            if (director)
            {
                director.Play();
            }
        }

        /// <summary>
        /// ステージのタイムラインを終了させる関数
        /// </summary>
        public void EndLevelDirector()
        {
            if (director)
            {
                director.Stop();
            }
        }
    }
}
