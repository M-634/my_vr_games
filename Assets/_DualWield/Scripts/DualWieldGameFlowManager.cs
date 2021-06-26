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

            //各ステージを初期化する
            foreach (var level in levelDatas)
            {
                level.ActiveLevel(false);
            }
        }

        private void TimeLine_StopAction(PlayableDirector _director)
        {
            if (_director.playableAsset == GameReadyStartPlayable)
            {
                //ステージのタイムラインを再生する。プレイヤーを動かす
                CurrentLevelData.PlayLevelDirector();
                playerControl.AutoMoveStart = true;
            }
            //リザルトが出た後
            else
            {
                //初期状態に戻る
            }
            director.playableAsset = null;
        }

        /// <summary>
        /// ステージ選択された時に呼ばれる関数。
        /// 一時的に画面を暗くし、その間にステージを出現させる。
        /// それが出来たら、画面を元に戻してそのステージを始める
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
            Debug.LogWarning("指定したステージがありません");
        }


        /// <summary>
        /// プレイヤーを止める。フェードアウト中にステージデータをクリアし、プレイヤーを初期地に戻す。
        /// fadeIn後にリザルトを出す
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

