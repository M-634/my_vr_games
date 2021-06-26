using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWield�̃X�^�[�g�A�N���A�A�Q�[���I�[�o�[���̗�����Ǘ�����N���X
    /// Timeline���x�[�X�ɊǗ�����B
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class DualWieldGameFlowManager : SingletonMonoBehaviour<DualWieldGameFlowManager>
    {
        [SerializeField] PlayableAsset GameReadyStartPlayable = default;
        [SerializeField] PlayableAsset GameClearPlayable = default;
        [SerializeField] PlayableAsset GameOverPlayable = default;
        [SerializeField] XRPlayerMoveControl playerControl;
        [SerializeField] List<LevelSettingSOData> levelDatas;

        ///<summary> false : GameOver , true : GameClear/// </summary>
        public Action<bool> EndGameAction;
        public LevelSettingSOData CurrentLevelData { get; private set; }

        PlayableDirector director;

        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            director.stopped += TimeLine_StopAction;
            EndGameAction += EndGame;

            //�e�X�e�[�W������������
            foreach (var level in levelDatas)
            {
                level.ActiveLevel(false);
            }
        }

        private void TimeLine_StopAction(PlayableDirector _director)
        {
            if (_director.playableAsset == GameReadyStartPlayable)
            {
                //�X�e�[�W�̃^�C�����C�����Đ�����B�v���C���[�𓮂���
                CurrentLevelData.PlayLevelDirector();
                playerControl.AutoMoveStart = true;
            }
            //���U���g���o����
            else
            {
                //������Ԃɖ߂�
            }
            director.playableAsset = null;
        }

        /// <summary>
        /// �X�e�[�W�I�����ꂽ���ɌĂ΂��֐��B
        /// �ꎞ�I�ɉ�ʂ��Â����A���̊ԂɃX�e�[�W���o��������B
        /// ���ꂪ�o������A��ʂ����ɖ߂��Ă��̃X�e�[�W���n�߂�
        /// </summary>
        public void OnSelectedLevel(int levelID = 0)
        {
            //fadeOut

            foreach (var level in levelDatas)
            {
                if (levelID == level.ID)
                {
                    level.ActiveLevel(true);
                    CurrentLevelData = level;
                    //fadeIn

                    GameReadyStart();
                    return;
                }
            }
            Debug.LogWarning("�w�肵���X�e�[�W������܂���");
        }


        /// <summary>
        /// �v���C���[���~�߂�B�t�F�[�h�A�E�g���ɃX�e�[�W�f�[�^���N���A���A�v���C���[�������n�ɖ߂��B
        /// fadeIn��Ƀ��U���g���o��
        /// </summary>
        private void EndGame(bool isGameClear)
        {
            playerControl.AutoMoveStart = false;
            //fadeOut
            playerControl.ResetPosition();
            CurrentLevelData.ActiveLevel(false);
            CurrentLevelData = null;
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
            director.PlayNullCheck(GameClearPlayable);
        }

        private void GameOver()
        {
            director.PlayNullCheck(GameOverPlayable);
        }
    }
}

