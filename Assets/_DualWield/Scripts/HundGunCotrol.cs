using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using TMPro;
using System.Threading;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// DualWied�̃��C������A�n���h�K���𐧌䂷��N���X
    /// </summary>
    public class HundGunCotrol : MonoBehaviour
    {
        [SerializeField] ActionBasedController controller;
        [SerializeField] string AnimatorShotName;

        [SerializeField] Transform muzzle;
        [SerializeField] float maxShotRange = 10f;
        [Tooltip("this is FireRate")]
        [SerializeField] float lineRendererDuration = 0.1f;
        [SerializeField] int maxAmmo = 10;
        [SerializeField] float canReloadAngle = 100f;
        [SerializeField] TextMeshProUGUI currentAmmoText = default;

        [SerializeField] AudioClip shot;
        [SerializeField] AudioClip reload;
        [SerializeField] AudioClip emptyAmmo;

        private int currentAmmo;
        private bool canShot = true;
        AudioSource audioSource;
        LineRenderer lineRenderer;
        Animator anim;

        public int CurrentAmmo 
        { 
            get => currentAmmo;
            set
            {
                currentAmmo = value;
                if (currentAmmoText)
                {
                    currentAmmoText.text = currentAmmo.ToString();
                }
            }
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            lineRenderer = GetComponent<LineRenderer>();
            audioSource = GetComponent<AudioSource>();

            lineRenderer.positionCount = 2;
            controller.activateAction.action.performed += Action_TriggerPressed;
            CurrentAmmo = maxAmmo;

            WaitReloadAction(this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// �v���C���[�������[�h�A�N�V����������܂ŁA�񓯊��őҋ@����B
        /// �A�N�V�������N�������烊���[�h�B
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        async UniTask WaitReloadAction(CancellationToken token)
        {
            while (true)
            {
                await UniTask.WaitUntil(() => ReloadAction(),PlayerLoopTiming.Update,token);
                CurrentAmmo = maxAmmo;
                if (audioSource)
                {
                    audioSource.Play(reload);
                }
            }
        }

        /// <summary>
        /// �e�����̊p�x���X�����烊���[�h���锻����s���֐�
        /// </summary>
        /// <returns></returns>
        private bool ReloadAction()
        {
            return Vector3.Angle(transform.up, Vector3.up) > canReloadAngle && CurrentAmmo < maxAmmo;
        }

        /// <summary>
        /// �R���g���[���[�̃g���K�[������������s�����֐�
        /// </summary>
        /// <param name="obj"></param>
        private void Action_TriggerPressed(InputAction.CallbackContext obj)
        {
            if (CurrentAmmo < 1)
            {
                CurrentAmmo = 0;
                audioSource.Play(emptyAmmo);
                return;
            }

            if (!canShot) return;

            if (anim)
            {
                anim.Play(AnimatorShotName);
                canShot = false;
            }
        }

        /// <summary>
        /// �A�j���[�V�����C�x���g����Ă΂��֐�
        /// </summary>
        public void Shot()
        {
            CurrentAmmo--; 
            if (audioSource)
            {
                audioSource.Play(shot);
            }
            DrawLine();
        }

        /// <summary>
        /// Ray���΂��āA�G�^�[�Q�b�g�ɓ������������ׂ�B����������A�G��|���B
        /// </summary>
        private async void DrawLine()
        {
            var isHit = Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, maxShotRange);
            if (isHit)
            {
                if (hit.transform.TryGetComponent(out IDamagable target))
                {
                    target.OnDamage();
                }
                SetLine(muzzle.position, hit.point);
            }
            else
            {
                SetLine(muzzle.position, muzzle.position + muzzle.forward * maxShotRange);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(lineRendererDuration), false, PlayerLoopTiming.LastUpdate, this.GetCancellationTokenOnDestroy());

            SetLine(muzzle.position, muzzle.position);
            canShot = true;
        }

        private void SetLine(Vector3 origin, Vector3 end)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, end);
        }
    }
}
