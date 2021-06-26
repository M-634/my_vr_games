using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

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
        [SerializeField] float lineRemdererDuration = 0.1f;

        [SerializeField] GameObject popUpScoreUI = default;


        LineRenderer lineRenderer;
        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.positionCount = 2;
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

        public void Shot()
        {
            DrawLine();
        }

        /// <summary>
        /// Rayを飛ばして、敵ターゲットに当たったか調べる。当たったら、敵を倒す。
        /// </summary>
        private async void DrawLine()
        {
            var isHit = Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, maxShotRange);
            if (isHit)
            {
                if (hit.transform.TryGetComponent(out IDamagable target))
                {
                    target.OnDamage();
                    if (popUpScoreUI)
                    {
                        popUpScoreUI.SetActive(true);
                        popUpScoreUI.transform.forward = transform.forward;
                    }
                }
                SetLine(muzzle.position, hit.point);
            }
            else
            {
                SetLine(muzzle.position, muzzle.position + muzzle.forward * maxShotRange);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(lineRemdererDuration), false, PlayerLoopTiming.FixedUpdate, this.GetCancellationTokenOnDestroy());

            SetLine(muzzle.position, muzzle.position);
            if (popUpScoreUI.activeSelf)
            {
                popUpScoreUI.SetActive(false);
            }
        }

        private void SetLine(Vector3 origin, Vector3 end)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, end);
        }
    }
}
