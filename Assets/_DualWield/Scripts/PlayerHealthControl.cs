using UnityEngine;

namespace Musahi.MY_VR_Games.DualWield
{
    public class PlayerHealthControl : MonoBehaviour, IDamagable
    {
        [SerializeField] float hp = 100f;

        [SerializeField] UnityEventsWrapper OnDamageEvents = default;
        [SerializeField] UnityEventsWrapper OnDieEvents = default;

        bool isDead = false;

        /// <summary>
        /// 敵の攻撃が当たる度に、体力を減らし、体力が「０」になったらゲームオーバー
        /// </summary>
        /// <param name="damage"></param>
        public void OnDamage(float damage = 0)
        {
            if (isDead) return;

            hp -= damage;
            if (hp <= 0)
            {
                OnDie();
                return;
            }

            if (OnDamageEvents != null)
            {
                OnDamageEvents.Invoke();
            }
        }


        private void OnDie()
        {
            isDead = true;

            if (OnDieEvents != null)
            {
                OnDieEvents.Invoke();
            }
            //ゲームオーバー
        }
    }
}