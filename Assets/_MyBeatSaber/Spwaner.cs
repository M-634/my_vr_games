using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Musahi.MY_VR_Games.MyBeatSaber
{
    public class Spwaner : MonoBehaviour
    {
        [SerializeField] GameObject[] nodes;
        [SerializeField] Transform[] points;
        [SerializeField] float beat;
        private float timer;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (timer > beat)
            {
                var node = Instantiate(nodes[Random.Range(0, 2)], points[Random.Range(0, 4)]);
                node.transform.localPosition = Vector3.zero;
                node.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));
                timer -= beat;
            }
            timer += Time.deltaTime;
        }
    }
}
