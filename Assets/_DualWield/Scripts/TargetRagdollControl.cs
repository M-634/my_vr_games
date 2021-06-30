using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Musahi.MY_VR_Games.DualWield
{
    /// <summary>
    /// ターゲット撃破時のコントローラークラス
    /// </summary>
    public class TargetRagdollControl : MonoBehaviour
    {
        /// <summary>吹き飛ぶ力（スカラ量）の最大値</summary>
        [SerializeField] float m_maxForce = 100f;
        /// <summary>吹き飛ぶ力（スカラ量）の最小値</summary>
        [SerializeField] float m_minForce = 50f;
        /// <summary>やられてから画面から消えるまでの秒数</summary>
        [SerializeField] float m_expirationPeriodAfterDeath = 5f;
        /// <summary>dissove の具合を調整するカーブ</summary>
        [SerializeField] AnimationCurve m_dissolveCurve;
        /// <summary>dissove のシェーダープロパティ</summary>
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
        /// プレイヤーと反対方向に吹っ飛ばす
        /// 子の Rigidbody のうちどれか一つに力を加える
        /// </summary>
        void BlowOff()
        {
            // 力を加える方向を決める
            GameObject player = GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 dir = this.transform.position - player.transform.position;  // プレイヤーと反対方向
            dir.y = 0;
            dir = dir.normalized * Random.Range(0f, 2f) + Vector3.up * Random.Range(-0.3f, 1f);
            dir = dir.normalized;

            // ランダムに部位を選び出し、力を加える
            Rigidbody[] rbArray = this.transform.GetComponentsInChildren<Rigidbody>();
            rbArray[Random.Range(0, rbArray.Length)].AddForce(dir * Random.Range(m_minForce, m_maxForce), ForceMode.Impulse);
        }

        /// <summary>
        /// m_dissolveCurve で指定したカーブに基づいて配下の material を dissolve する
        /// 全ての material にはシェーダーに DissolveEmission を指定すること
        /// dissolve 率が 99% になった時点でオブジェクトを破棄する
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
