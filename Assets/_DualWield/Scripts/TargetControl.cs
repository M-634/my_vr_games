using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Cysharp;
using System;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// 敵ターゲットを管理するクラス
    /// 追いかけては来ない
    /// </summary>
    public class TargetControl : MonoBehaviour, IDamagable
    {
        [SerializeField] Transform player;
        [SerializeField] float attackRange = 5f;
        [SerializeField] Color attackRangeColor = default;
        [SerializeField, Range(0f, 1f)] float lookDuration = 0.2f;
        [SerializeField] float fireRate = 1f;

        [SerializeField] Transform muzzle;
        [SerializeField] Bullet bulletPrefab;

        [SerializeField] UnityEventsWrapper OnDieEvents = default;
        [SerializeField] bool isTest = false;
        bool isDead = false;
        float lastAttackTime;

        private void Start()
        {
            if (!player)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }


        private void Update()
        {
            if (CanAttackPlayer())
            {
                LookAtPlayer();
                if(Time.time > lastAttackTime + fireRate)
                {
                    Shot();
                }
            }
        }

        private void Shot()//後でObjectPoolに変える
        {
            var b = Instantiate(bulletPrefab,muzzle.position,Quaternion.identity);
            b.SetVelocity(muzzle);
            lastAttackTime = Time.time;
        }

        private bool CanAttackPlayer()
        {
            Vector3 dir = player.position - transform.position;
            if (dir.magnitude < attackRange)
            {
                return true;
            }
            return false;
        }
        public void LookAtPlayer()
        {
            var aim = player.position - transform.position;
            aim.y = 0;
            var look = Quaternion.LookRotation(aim);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, lookDuration);
        }

        /// <summary>
        /// ターゲットは、銃弾が当たったら一撃で倒れる
        /// </summary>
        /// <param name="damage"></param>
        public void OnDamage(float damage = 0)
        {
            if (isDead) return;
            OnDie();
        }

        private void OnDie()
        {
            if (OnDieEvents != null)
            {
                OnDieEvents.Invoke();
            }

            if (!isTest)
            {
                isDead = true;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = attackRangeColor;
            Handles.DrawSolidDisc(transform.position, Vector3.up, attackRange);
        }
    }
#endif
}