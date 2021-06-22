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

        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            controller.activateAction.action.performed += Action_TriggerPressed;
        }

        private void Action_TriggerPressed(InputAction.CallbackContext obj)
        {
            if (anim)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(AnimatorShotName)) return;
                anim.Play(AnimatorShotName);
            }
        }

        /// <summary>
        /// アニメーションイベントから呼ばれる
        /// </summary>
        public void Shot()
        {
            Debug.Log("shot!!");
        }
    }
}
