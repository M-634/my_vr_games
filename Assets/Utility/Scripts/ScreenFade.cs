using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.Events;

namespace Musahi.MY_VR_Games
{
    ///memo �F �V���O���p�X�ŕ`�悵�������AFadeShader���V���O���p�X�ɑΉ������悤�Ƃ��Ă���肭�����Ȃ����߁A
    ///�ꎞ�I�Ƀ}���`�p�X�ɂ��Ă���B
    /// <summary>
    /// URP�̃����_�[�p�C�v���C���𗘗p�����X�N���[���̃t�F�[�h�@�\
    /// </summary>
    public class ScreenFade : SingletonMonoBehaviour<ScreenFade>
    {
        [SerializeField] ForwardRendererData rendererData = default;

        [SerializeField, Range(0, 1)] float alpha = 1.0f;
        [SerializeField, Range(0, 5)] float duration = 0.5f;

        [SerializeField] string fadePropertyNameOfShader = "_Alpha";
        private Material fadeMaterial = default;


        void Start()
        {
            SetUpFadeFeature();
        }

        private void SetUpFadeFeature()
        {
            ScriptableRendererFeature feature = rendererData.rendererFeatures.Find(item => item is ScreenFadeFeature);
            
            if(feature is ScreenFadeFeature screenFade)
            {
                //Duplicate material so we don't change the renderer's asset
                fadeMaterial = Instantiate(screenFade.settings.material);
                screenFade.settings.runTimeMaterial = fadeMaterial;
            }
        }

        public void FadeIn(UnityAction callback = null)
        {
            DOTween.To(() => alpha = 1f, (x) => alpha = x, 0f, duration)
                .OnUpdate(() => fadeMaterial.SetFloat(fadePropertyNameOfShader, alpha))
                .OnComplete(() => callback?.Invoke()); 
        }

        public void FadeOut(UnityAction callback = null)
        {
            DOTween.To(() => alpha = 0f, (x) => alpha = x, 1f, duration)
               .OnUpdate(() => fadeMaterial.SetFloat(fadePropertyNameOfShader, alpha))
               .OnComplete(() => callback?.Invoke());
        }
    }
}
