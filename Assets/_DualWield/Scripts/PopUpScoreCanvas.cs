using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// 敵撃破時のスコアをポップアップ形式で表示する
    /// </summary>
    public class PopUpScoreCanvas : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] float addYValue = 0.5f;
        [SerializeField] float moveDuration = 0.2f;
        [SerializeField] float delayDuration = 2f;
        float initPosY;

        private void Awake()
        {
            initPosY = transform.position.y;
        }

        private void OnEnable()
        {
            transform.position = new Vector3(target.position.x, initPosY, target.position.z);
            StartCoroutine(BillBoard());
            transform.DOLocalMoveY(initPosY + addYValue, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(()=> gameObject.DelaySetActive(false,delayDuration).Forget());
        }

        /// <summary>
        /// 敵撃破時のイベントから呼ばれる
        /// </summary>
        /// <param name="getScore"></param>
        public void SetScore(int getScore)
        {
            scoreText.text = getScore.ToString();
            //スコアを追加する
            DualWieldGameFlowManager.Instance.CurrentLevelResultSumScore += getScore;
        }

        IEnumerator BillBoard()
        {
            while (gameObject.activeSelf)
            {
                var dir = transform.position - Camera.main.transform.position;
                dir.y = 0;
                transform.forward = dir.normalized;
                yield return null;
            }
        }
    }
}