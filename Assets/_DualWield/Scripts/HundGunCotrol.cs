using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWiedのメイン武器、ハンドガンを制御するクラス
    /// </summary>
    public class HundGunCotrol : MonoBehaviour
    {
        [SerializeField] ActionBasedController controller;
        [SerializeField] string AnimatorShotName;

        [SerializeField] Transform muzzle;
        [SerializeField] float maxShotRange = 10f;


        LineRenderer lineRenderer;
        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.SetPositions(new Vector3[] { muzzle.position, muzzle.position });
            controller.activateAction.action.performed += Action_TriggerPressed;
        }

        private void Action_TriggerPressed(InputAction.CallbackContext obj)
        {
            if (anim)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(AnimatorShotName)) return;//銃を撃つアニメーションが再生中は、トリガー押しても何も起こらない。
                anim.Play(AnimatorShotName);
            }
        }

        /// <summary>
        /// アニメーションイベントから呼ばれる。
        /// Rayを飛ばして、敵ターゲットに当たったか調べる。当たったら、敵を倒す。
        /// </summary>
        public void Shot()
        {
            DrawLine();
        }


        private async void DrawLine()
        {
            var isHit = Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, maxShotRange);

            if (isHit)
            {
                if(hit.transform.TryGetComponent(out IDamagable target))
                {
                    target.OnDamage();
                }
                lineRenderer.SetPosition(1, hit.point);
            }
            lineRenderer.SetPosition(1, muzzle.position + muzzle.forward * maxShotRange);

        
        }
    }
}
