using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Musahi.MY_VR_Games
{
    /// <summary>
    /// XRコントローラーの移動を制御するクラス
    /// </summary>
    public class XRPlayerMoveControl : MonoBehaviour
    {
        [SerializeField] XRNode inputSorce;
        [SerializeField] float moveSpeed = 5.0f;
        [SerializeField] float gravityScale = 9.81f;
        [SerializeField] LayerMask groudLayer;
        [SerializeField] float additionalHeight = 0.2f;

        XRRig rig;
        CharacterController characterController;
        Vector2 inputAxis;
        Vector3 velocity;
     
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            rig = GetComponent<XRRig>();
        }

        void Update()
        {
            var device = InputDevices.GetDeviceAtXRNode(inputSorce);
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        }

        private void FixedUpdate()
        {
            Move();
        }


        /// <summary>
        /// コントローラーのスティック移動
        /// </summary>
        private void Move()
        {
            CapsuleFollowHMD();

            if (CheakGround())
            {
                //HMDが向いている方向に進む。
                var headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
                var dir = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y).normalized;
                velocity = dir * moveSpeed;
                characterController.Move(velocity * Time.fixedDeltaTime);
            }
            else
            {
                velocity += gravityScale * Time.fixedDeltaTime * Vector3.down;
                characterController.Move(velocity * Time.fixedDeltaTime);
            }
        }

        private bool CheakGround()
        {
            var ratStart = transform.TransformPoint(characterController.center);
            var rayLength = characterController.center.y + 0.01f;
            return Physics.SphereCast(ratStart, characterController.radius, Vector3.down, out RaycastHit hit, rayLength, groudLayer);
        }

        private void CapsuleFollowHMD()
        {
            characterController.height = rig.cameraInRigSpaceHeight + additionalHeight;
            Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
            characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
        }
    }
}
