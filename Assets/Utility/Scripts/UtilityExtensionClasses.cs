using UnityEngine;
using UnityEngine.Playables;
using System.Threading;
using Cysharp.Threading.Tasks;

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
        /// <summary>
        /// アクティブの切り替えを遅延させる拡張メソッド
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask DelaySetActive(this GameObject gameObject,bool value, float duration = 0f,CancellationToken token = default)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration), false, PlayerLoopTiming.Update, token);
            gameObject.SetActive(value);
        }
    }
}
