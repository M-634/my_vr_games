using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Musahi.MY_VR_Games.DualWield
{
    public class GoalTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                DualWieldGameFlowManager.Instance.EndGameAction.Invoke(true);
            }       
        }
    }
}