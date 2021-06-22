using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Musahi.MY_VR_Games
{
    public enum PlayerMovementType
    {
        NoController,//�R���g���[���[�ňړ����Ȃ�
        Teleport,//�e���|�[�g�ňړ�����
        Stick//�R���g���[���[�̃X�e�B�b�N�ړ� 
    }

    /// <summary>
    /// XR�R���g���[���[�̈ړ��𐧌䂷��N���X
    /// </summary>
    public class XRPlayerMoveControl : MonoBehaviour
    {
        [SerializeField] PlayerMovementType movementType;
        [SerializeField] XRNode inputSource;
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
            if (movementType == PlayerMovementType.NoController) return;

            var device = InputDevices.GetDeviceAtXRNode(inputSource);
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        }

        private void FixedUpdate()
        {
            CapsuleFollowHMD();

            switch (movementType)
            {
                case PlayerMovementType.NoController:
                    break;
                case PlayerMovementType.Teleport:
                    break;
                case PlayerMovementType.Stick:
                    StickMovement();
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// �R���g���[���[�̃X�e�B�b�N�ړ�
        /// </summary>
        private void StickMovement()
        {
            if (CheakGround())
            {
                //HMD�������Ă�������ɐi�ށB
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
