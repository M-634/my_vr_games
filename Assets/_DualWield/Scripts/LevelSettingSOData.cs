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
        [SerializeField] PlayableDirector director;

        public int ID => levelID;
        public GameObject Level => levelPrefab;
        public PlayableDirector GetDirector => director;

        /// <summary>
        /// �X�e�[�W�I�u�W�F�N�g�S�̂̃A�N�e�B�u��؂�ւ���֐�
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
        /// �X�e�[�W�̃^�C�����C�����Đ��֐�
        /// </summary>
        public void PlayLevelDirector()
        {
            if (director)
            {
                director.Play();
            }
        }

        /// <summary>
        /// �X�e�[�W�̃^�C�����C�����I��������֐�
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
