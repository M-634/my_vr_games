using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Musahi.MY_VR_Games.MyBeatSaber
{
    public class DestroyObject : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}