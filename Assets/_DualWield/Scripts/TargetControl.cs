using UnityEngine;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// 敵ターゲットを管理するクラス
    /// </summary>
    public class TargetControl : MonoBehaviour, IDamagable
    {
        [SerializeField] UnityEventsWrapper OnDieEvents = default;
        [SerializeField] bool isDebug = false;

        bool isDead = false; 

        /// <summary>
        /// ターゲットは、銃弾が当たったら一撃で倒れる
        /// </summary>
        /// <param name="damage"></param>
        public void OnDamage(float damage = 0)
        {
            if (isDead && !isDebug) return;

            if(OnDieEvents != null)
            {
                OnDieEvents.Invoke();
            }
            isDead = true;
        }
    }
}