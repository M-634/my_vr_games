using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWieldのスタート、クリア、ゲームオーバー等の流れと獲得スコアを管理するクラス。
    /// Timelineをベースに管理する。また、スコアに応じてランクを付けて保存する
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
            public List<TargetControl> targetsList;
            public InstantiateLevelData(int id, GameObject gameObject, PlayableDirector director)
            {
                LevelId = id;
                LevelObject = gameObject;
                LevelDirector = director;
                targetsList = new List<TargetControl>();
            }
            public void LevelActive(bool value)
            {
                if (LevelObject)
                {
                    LevelObject.SetActive(value);
                }
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

                foreach (var target in targetsList)
                {
                    target.Initialize();
                }
            }
            public void AddTarget(TargetControl target)
            {
                if (!targetsList.Contains(target))
                {
                    targetsList.Add(target);
                }
            }
        }

        [SerializeField] PlayableAsset GameReadyStartPlayable = default;
        [SerializeField] PlayableAsset GameClearPlayable = default;
        [SerializeField] PlayableAsset GameOverPlayable = default;

        [SerializeField] TextMeshProUGUI resultScoreText = default;
        [SerializeField] TextMeshProUGUI resultRankText = default;

        [SerializeField] XRPlayerMoveControl playerControl;
        [SerializeField] PlayerHealthControl playerHealth;
        [SerializeField] List<LevelSettingSOData> levelDatas;


        [SerializeField]//test
        public InstantiateLevelData CurrentLevelData;// { get; private set; }
        private readonly List<InstantiateLevelData> instantiateLevelDataList = new List<InstantiateLevelData>();
        PlayableDirector director;

        /// <summary>現在遊んでいるレベルのリザルトスコア</summary>
        public int CurrentLevelResultSumScore { get; set; }

        private void Start()
        {
            ScreenFade.Instance.FadeIn();

            director = GetComponent<PlayableDirector>();
            director.stopped += TimeLine_StopAction;

            //各ステージを初期化する
            foreach (var data in levelDatas)
            {
                SetLevelData(data);
            }
        }

        /// <summary>
        /// インスタンス化したレベルデータをセットする
        /// </summary>
        /// <param name="data"></param>
        private void SetLevelData(LevelSettingSOData data)
        {
            var levelPrefab = Instantiate(data.GetLevelPrefab);
            levelPrefab.TryGetComponent(out PlayableDirector levelDirector);
            var instanceLevelData = new InstantiateLevelData(data.GetID, levelPrefab, levelDirector);
            instanceLevelData.LevelActive(false);
            instantiateLevelDataList.Add(instanceLevelData);
        }

        private void TimeLine_StopAction(PlayableDirector _director)
        {
            if (_director.playableAsset == GameReadyStartPlayable)
            {
                //スコアをリセットする
                CurrentLevelResultSumScore = 0;
                //ステージのタイムラインを再生する。プレイヤーを動かす
                playerControl.AutoMoveStart = true;
                CurrentLevelData.PlayLevelDirector();
            }
            //リザルトが出た後
            else
            {
                //初期状態に戻る
                Debug.Log("result");
                CurrentLevelData = null;
            }
            director.playableAsset = null;
        }

        /// <summary>
        /// ステージ選択された時に呼ばれる関数。
        /// 一時的に画面を暗くし、その間にステージを出現させる。
        /// それが出来たら、画面を元に戻してそのステージを始める
        /// </summary>
        public void OnSelectedLevel(int getID = 0)
        {
            ScreenFade.Instance.FadeOut(() =>
            {
                foreach (var data in instantiateLevelDataList)
                {
                    if (getID == data.LevelId)
                    {
                        data.LevelActive(true);
                        CurrentLevelData = data;
                        ScreenFade.Instance.FadeIn(() => GameReadyStart());
                        return;
                    }
                }
                Debug.LogWarning("指定したIDのステージが存在しません");
            });

        }

        /// <summary>
        /// プレイヤーを止める。フェードアウト中にステージデータをクリアし、プレイヤーを初期地に戻す。
        /// fadeIn後にリザルトを出す
        /// </summary>
        public void EndGame(bool isGameClear)
        {
            CurrentLevelData.StopLevelDirector();
            playerControl.AutoMoveStart = false;

            ScreenFade.Instance.FadeOut(() =>
            {
                playerControl.ResetPosition();
                playerHealth.ResetHitCount();
                CurrentLevelData.LevelActive(false);

                ScreenFade.Instance.FadeIn(() =>
                {
                    if (isGameClear)
                    {
                        GameClear();
                    }
                    else
                    {
                        GameOver();
                    }
                });
            });


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
                resultRankText.text = "Rank :" + DetermineRank();
            }
            director.PlayNullCheck(GameClearPlayable);
        }
        private void GameOver()
        {
            director.PlayNullCheck(GameOverPlayable);
        }

        /// <summary>/// リザルトスコアに応じてランクを決める/// </summary>
        private string DetermineRank()
        {
            return "";
        }

    }
}

