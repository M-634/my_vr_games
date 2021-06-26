using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Musahi.MY_VR_Games
{
    public abstract class SingletonMonoBehaviour<T>: MonoBehaviour where T:MonoBehaviour 
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogWarning(t + "���A�^�b�`���Ă���GameObject�͂���܂���");
                    }
                }
                return instance;
            }
        }

        virtual protected void Awake()
        {
            //����GameObject�ɃA�^�b�`����Ă��邩���ׂ�
            //�A�^�b�`����Ă���ꍇ�͔j������
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
