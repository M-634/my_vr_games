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
    public class DualWieldGameFlowManager : MonoBehaviour
    {
        [SerializeField] PlayableAsset GameReadyStartPlayable = default;
        [SerializeField] PlayableAsset GameClearPlayable = default;
        [SerializeField] PlayableAsset GameOverPlayable = default;
        [SerializeField] XRPlayerMoveControl playerControl;

        public Action GameStartStageAction;
        public Action<bool> EndGameAction;
        PlayableDirector director;

        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            director.stopped += TimeLine_StopAction;
            EndGameAction += EndGame;

            //test
            GameReadyStart();
        }

        private void TimeLine_StopAction(PlayableDirector _director)
        {
            if(_director.playableAsset == GameReadyStartPlayable)
            {
                //�X�e�[�W�̃^�C�����C�����Đ�����B�v���C���[�𓮂���
                playerControl.AutoMoveStart = true;
                if(GameStartStageAction != null)
                {
                    GameStartStageAction.Invoke();
                }
            }
            else
            {
                //������Ԃɖ߂�
            }
            director.playableAsset = null;
        }

        private void GameReadyStart()
        {
            director.PlayNullCheck(GameReadyStartPlayable);
        }

        /// <summary>
        /// �v���C���[���~�߂�B�G��S�ď����B
        /// �V�[����������Ԃɖ߂��āA���U���g���o��
        /// </summary>
        private void EndGame(bool isGameClear = false)
        {
            playerControl.AutoMoveStart = false;
            if (isGameClear)
            {
                GameClear();
            }
            else
            {
                GameOver();
            }
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
namespace Musahi.MY_VR_Games
{
    public static class UtilityExtensionClasses
    {
        /// <summary>
        ///  PlayableDirector�N���X�̊g�����\�b�h�B
        ///  PlayableAsset��Null�`�F�b�N
        /// </summary>
        /// <param name="director"></param>
        /// <param name="playableAsset"></param>
        public static void PlayNullCheck(this PlayableDirector director, PlayableAsset playableAsset)
        {
            if (playableAsset)
            {
                director.Play(playableAsset);
            }
            else
            {
                Debug.LogWarning($"{playableAsset}���A�T�C������Ă��܂���I");
            }
        }
    }
}
