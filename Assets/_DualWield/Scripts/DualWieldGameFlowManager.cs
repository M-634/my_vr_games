using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWield�̃X�^�[�g�A�N���A�A�Q�[���I�[�o�[���̗���Ɗl���X�R�A���Ǘ�����N���X�B
    /// Timeline���x�[�X�ɊǗ�����B�܂��A�X�R�A�ɉ����ă����N��t���ĕۑ�����
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class DualWieldGameFlowManager : SingletonMonoBehaviour<DualWieldGameFlowManager>
    {
        [Serializable]
        public class InstantiateLevelData
        {
            public int LevelId;// { get; private set; }
            public GameObject LevelObject;// { get; private set; }
            public PlayableDirector LevelDirector;// { get; private set; }
            public TargetControl[] LevelTargets;// = enemy

            public InstantiateLevelData(int id, GameObject gameObject, PlayableDirector director,TargetControl[] targets)
            {
                LevelId = id;
                LevelObject = gameObject;
                LevelDirector = director;
                LevelTargets = targets;
            }

            public void LevelActive(bool value)
            {
                if (LevelObject)
                {
                    LevelObject.SetActive(value);
                }
                SetTargetsActive(value);
            }

            public void PlayLevelDirector()
            {
                if (LevelDirector)
                {
                    LevelDirector.Play();
                }
            }

            public void StopLevelDirector()
            {
                if (LevelDirector)
                {
                    LevelDirector.Stop();
                }
            }

            public void SetTargetsActive(bool value)
            {
                foreach (var target in LevelTargets)
                {
                    if (value)
                    {
                        target.InitializeTransform();
                    }
                    target.gameObject.SetActive(value);
                }
            }
        }

        [SerializeField] PlayableAsset GameReadyStartPlayable = default;
        [SerializeField] PlayableAsset GameClearPlayable = default;
        [SerializeField] PlayableAsset GameOverPlayable = default;

        [SerializeField] TextMeshProUGUI resultScoreText = default;
        [SerializeField] TextMeshProUGUI resultRankText = default;

        [SerializeField] XRPlayerMoveControl playerControl;
        [SerializeField] List<LevelSettingSOData> levelDatas;



        [SerializeField]//test
        public InstantiateLevelData CurrentLevelData;// { get; private set; }
        private readonly List<InstantiateLevelData> instantiateLevelDataList = new List<InstantiateLevelData>();
        PlayableDirector director;

        /// <summary>���ݗV��ł��郌�x���̃��U���g�X�R�A</summary>
        public int CurrentLevelResultSumScore { get; set; }

        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            director.stopped += TimeLine_StopAction;

            //�e�X�e�[�W������������
            foreach (var data in levelDatas)
            {
                SetLevelData(data);
            }
        }

        /// <summary>
        /// �C���X�^���X���������x���f�[�^���Z�b�g����
        /// </summary>
        /// <param name="data"></param>
        private void SetLevelData(LevelSettingSOData data)
        {
            var levelPrefab = Instantiate(data.GetLevelPrefab);
            levelPrefab.TryGetComponent(out PlayableDirector levelDirector);
            var levelTargets = levelPrefab.GetComponentsInChildren<TargetControl>();
            var instanceLevelData = new InstantiateLevelData(data.GetID, levelPrefab, levelDirector,levelTargets);
            instanceLevelData.LevelActive(false);
            instantiateLevelDataList.Add(instanceLevelData);
        }

        private void TimeLine_StopAction(PlayableDirector _director)
        {
            if (_director.playableAsset == GameReadyStartPlayable)
            {
                //�X�R�A�����Z�b�g����
                CurrentLevelResultSumScore = 0;
                //�X�e�[�W�̃^�C�����C�����Đ�����B�v���C���[�𓮂���
                playerControl.AutoMoveStart = true;
                CurrentLevelData.PlayLevelDirector();
            }
            //���U���g���o����
            else
            {
                //������Ԃɖ߂�
                Debug.Log("result");
            }
            director.playableAsset = null;
        }

        /// <summary>
        /// �X�e�[�W�I�����ꂽ���ɌĂ΂��֐��B
        /// �ꎞ�I�ɉ�ʂ��Â����A���̊ԂɃX�e�[�W���o��������B
        /// ���ꂪ�o������A��ʂ����ɖ߂��Ă��̃X�e�[�W���n�߂�
        /// </summary>
        public void OnSelectedLevel(int getID = 0)
        {
            //fadeOut

            foreach (var data in instantiateLevelDataList)
            {
                if (getID == data.LevelId)
                {
                    data.LevelActive(true);
                    CurrentLevelData = data;
                    //fadeIn

                    GameReadyStart();
                    return;
                }
            }
            Debug.LogWarning("�w�肵��ID�̃X�e�[�W�����݂��܂���");
        }

        /// <summary>
        /// �v���C���[���~�߂�B�t�F�[�h�A�E�g���ɃX�e�[�W�f�[�^���N���A���A�v���C���[�������n�ɖ߂��B
        /// fadeIn��Ƀ��U���g���o��
        /// </summary>
        public void EndGame(bool isGameClear)
        {
            CurrentLevelData.StopLevelDirector();
            playerControl.AutoMoveStart = false;
         
            //fadeOut
            playerControl.ResetPosition();
            CurrentLevelData.LevelActive(false);
            //fadeIn
            if (isGameClear)
            {
                GameClear();
            }
            else
            {
                GameOver();
            }
        }

        private void GameReadyStart()
        {
            director.PlayNullCheck(GameReadyStartPlayable);
        }

        private void GameClear()
        {
            if (resultScoreText)
            {
                resultScoreText.text = "Score : " + CurrentLevelResultSumScore.ToString();
            }
            if (resultRankText)
            {
                resultRankText.text = "Rank :" +  DetermineRank();
            }
            director.PlayNullCheck(GameClearPlayable);
        }
        private void GameOver()
        {
            director.PlayNullCheck(GameOverPlayable);
        }

        /// <summary>/// ���U���g�X�R�A�ɉ����ă����N�����߂�/// </summary>
        private string DetermineRank()
        {
            return "";
        }

    }
}

