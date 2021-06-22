using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// XR�̃C���v�b�g���Ǘ�����N���X�i���b�p�[�N���X�j
/// </summary>
public class XRInputProvider : MonoBehaviour
{
    public Vector2 GetInputAxis(XRNode inputSorce)
    {
        var device = InputDevices.GetDeviceAtXRNode(inputSorce);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxis);
        return inputAxis;
    }
}
