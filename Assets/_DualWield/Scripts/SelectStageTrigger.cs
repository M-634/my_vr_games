using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Musahi.MY_VR_Games.DualWield
{
    public class SelectStageTrigger : MonoBehaviour, IDamagable
    {
        [SerializeField] int levelID = 0;
        [SerializeField] UnityEventsWrapper OnSelectEvents = default;
        public void OnDamage(float damage = 0)
        {
            if(OnSelectEvents != null)
            {
                OnSelectEvents.Invoke();
            }
            DualWieldGameFlowManager.Instance.OnSelectedLevel(levelID);
        }
    }
}