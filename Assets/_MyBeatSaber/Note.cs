using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Musahi.MY_VR_Games.MyBeatSaber
{
    public class Note : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position += Time.deltaTime * transform.forward * 2;
        }
    }
}
