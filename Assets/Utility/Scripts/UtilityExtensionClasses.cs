using UnityEngine;
using UnityEngine.Playables;

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
