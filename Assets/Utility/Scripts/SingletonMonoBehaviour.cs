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
                        Debug.LogWarning(t + "をアタッチしているGameObjectはありません");
                    }
                }
                return instance;
            }
        }

        virtual protected void Awake()
        {
            //他のGameObjectにアタッチされているか調べる
            //アタッチされている場合は破棄する
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
