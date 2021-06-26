using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWieldのスタート、クリア、ゲームオーバー等の流れを管理するクラス
    /// Timelineをベースに管理する。
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
                //ステージのタイムラインを再生する。プレイヤーを動かす
                playerControl.AutoMoveStart = true;
                if(GameStartStageAction != null)
                {
                    GameStartStageAction.Invoke();
                }
            }
            else
            {
                //初期状態に戻る
            }
            director.playableAsset = null;
        }

        private void GameReadyStart()
        {
            director.PlayNullCheck(GameReadyStartPlayable);
        }

        /// <summary>
        /// プレイヤーを止める。敵を全て消す。
        /// シーンを初期状態に戻して、リザルトを出す
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
        ///  PlayableDirectorクラスの拡張メソッド。
        ///  PlayableAssetのNullチェック
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
                Debug.LogWarning($"{playableAsset}がアサインされていません！");
            }
        }
    }
}
