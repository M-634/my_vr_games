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
    public class TargetControl : MonoBehaviour, IDamagable, IPoolUser<TargetControl>
    {
        [SerializeField] Transform player;
        [SerializeField] float attackRange = 5f;
        [SerializeField] Color attackRangeColor = default;
        [SerializeField, Range(0f, 1f)] float lookDuration = 0.2f;
        [SerializeField] float fireRate = 1f;

        [SerializeField] Transform muzzle;
        [SerializeField] Bullet bulletPrefab;
        [SerializeField, Range(1, 100)] int bulletPoolSize = 10;
        [SerializeField] Transform poolObjectParent;

        [SerializeField] bool IsIkActive;
        [SerializeField] Transform targetRightHand;
        [SerializeField] Transform targetLeftHand;

        [SerializeField] UnityEventsWrapper OnDieEvents = default;

        [SerializeField] bool isTest = false;

        bool isDead = false;
        float lastAttackTime;
        PoolObjectManager poolObjectManager;
        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            if (!player)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            InitializePoolObject(bulletPoolSize);
        }

        public void InitializePoolObject(int poolSize = 1)
        {
            poolObjectManager = new PoolObjectManager();

            if (!poolObjectParent)
            {
                poolObjectParent = transform;
            }

            for (int i = 0; i < poolSize; i++)
            {
                SetPoolObj();
            }
        }

        public PoolObjectManager.PoolObject SetPoolObj()
        {
            var poolObj = poolObjectManager.InstantiatePoolObj();

            var bullet = Instantiate(bulletPrefab, poolObjectParent);

            poolObj.AddObj(bullet.gameObject);
            poolObj.SetActiveAll(false);
            return poolObj;
        }


        private void Update()
        {
            if (CanAttackPlayer())
            {
                LookAtPlayer();
                IsIkActive = true;
                if (Time.time > lastAttackTime + fireRate)
                {
                    Shot();
                }
            }
            else
            {
                IsIkActive = false;
            }
        }

        private void Shot()
        {
            poolObjectManager.UsePoolObject(muzzle.position, muzzle.rotation, SetPoolObj);
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

        private void OnAnimatorIK(int layerIndex)
        {
            if (!IsIkActive) return;

            if (targetLeftHand == null || targetRightHand == null) return;

            // 両手の IK Position/Rotation をセットする
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, targetRightHand.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, targetRightHand.rotation);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHand.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, targetLeftHand.rotation);
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