using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// �^�[�Q�b�g���j���̃R���g���[���[�N���X
    /// </summary>
    public class TargetRagdollControl : MonoBehaviour
    {
        /// <summary>������ԗ́i�X�J���ʁj�̍ő�l</summary>
        [SerializeField] float m_maxForce = 100f;
        /// <summary>������ԗ́i�X�J���ʁj�̍ŏ��l</summary>
        [SerializeField] float m_minForce = 50f;
        /// <summary>����Ă����ʂ��������܂ł̕b��</summary>
        [SerializeField] float m_expirationPeriodAfterDeath = 5f;
        /// <summary>dissove �̋�𒲐�����J�[�u</summary>
        [SerializeField] AnimationCurve m_dissolveCurve;
        /// <summary>dissove �̃V�F�[�_�[�v���p�e�B</summary>
        [SerializeField] string m_dissolvePropertyNameOfShader = "_Dissolve";
        float m_timer;

        SkinnedMeshRenderer m_meshRenderer;
        Material[] m_materials;


        void Start()
        {
            m_meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            m_materials = m_meshRenderer.sharedMaterials;

            BlowOff();
        }


        void Update()
        {
            Dissolve();
            m_timer += Time.deltaTime;
        }

        /// <summary>
        /// �v���C���[�Ɣ��Ε����ɐ�����΂�
        /// �q�� Rigidbody �̂����ǂꂩ��ɗ͂�������
        /// </summary>
        void BlowOff()
        {
            // �͂���������������߂�
            GameObject player = GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 dir = this.transform.position - player.transform.position;  // �v���C���[�Ɣ��Ε���
            dir.y = 0;
            dir = dir.normalized * Random.Range(0f, 2f) + Vector3.up * Random.Range(-0.3f, 1f);
            dir = dir.normalized;

            // �����_���ɕ��ʂ�I�яo���A�͂�������
            Rigidbody[] rbArray = this.transform.GetComponentsInChildren<Rigidbody>();
            rbArray[Random.Range(0, rbArray.Length)].AddForce(dir * Random.Range(m_minForce, m_maxForce), ForceMode.Impulse);
        }

        /// <summary>
        /// m_dissolveCurve �Ŏw�肵���J�[�u�Ɋ�Â��Ĕz���� material �� dissolve ����
        /// �S�Ă� material �ɂ̓V�F�[�_�[�� DissolveEmission ���w�肷�邱��
        /// dissolve ���� 99% �ɂȂ������_�ŃI�u�W�F�N�g��j������
        /// </summary>
        void Dissolve()
        {
            float value = m_dissolveCurve.Evaluate(m_timer / m_expirationPeriodAfterDeath);

            //if (m_material.HasProperty(m_dissolvePropertyNameOfShader))
            //{
            //    m_material.SetFloat(m_dissolvePropertyNameOfShader, value);
            //}

            foreach (var material in m_materials)
            {
                if (material.HasProperty(m_dissolvePropertyNameOfShader))
                {
                    material.SetFloat(m_dissolvePropertyNameOfShader, value);
                }
            }

            if (value > .99f)
            {
                Destroy(gameObject);
            }
        }
    }
}
