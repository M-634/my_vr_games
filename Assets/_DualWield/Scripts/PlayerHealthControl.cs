using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace Musahi.MY_VR_Games.DualWield
{
    public class PlayerHealthControl : MonoBehaviour, IDamagable
    {
        [SerializeField] int maxHitCount = 5;

        [SerializeField] UnityEventsWrapper OnDamageEvents = default;
        [SerializeField] UnityEventsWrapper OnDieEvents = default;


        [SerializeField] VolumeProfile volumeProfile = default;
        [SerializeField] Vignette vignette;

        int currentHitCount;
        bool isDead = false;

        public int CurrentHitCount { 
            get=> currentHitCount;
            set 
            {
                currentHitCount = value;
                vignette.intensity.Override(1 - (float)currentHitCount / maxHitCount);
            }
        }

        private void Start()
        {
            volumeProfile.TryGet(out vignette);
            ResetHitCount();
        }

        public void ResetHitCount()
        {
            CurrentHitCount = maxHitCount;
        }


        /// <summary>
        /// 敵の攻撃が当たる度に、体力を減らし、体力が「０」になったらゲームオーバー
        /// </summary>
        /// <param name="damage"></param>
        public void OnDamage(float damage = 0)
        {
            if (isDead) return;

            CurrentHitCount--;
            if(CurrentHitCount == 0)
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