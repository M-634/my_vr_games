using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Musahi.MY_VR_Games.MyBeatSaber
{
    public class Saber : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float sliceLimitAngle = 130;
        private Vector3 previousPos;
        private RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            previousPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(HitCheckNode() && CanSliceAngle())
            {
               Destroy(hit.transform.gameObject);
            }
            previousPos = transform.position;
        }

        private bool HitCheckNode()
        {
            return Physics.Raycast(transform.position, transform.forward, out hit, 0.1f,layerMask);
        }

        private bool CanSliceAngle()
        {
            return Vector3.Angle(transform.position - previousPos, hit.transform.up) > sliceLimitAngle;
        }
    }
}
