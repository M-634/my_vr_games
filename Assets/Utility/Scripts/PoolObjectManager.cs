using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Musahi.MY_VR_Games
{
    /// <summary>
    /// �I�u�W�F�b�g�v�[����K�p����GameObject���܂Ƃ߂Đ��䂷��N���X
    /// </summary>
    public class PoolObjectManager
    {
        /// <summary>
        /// �v�[���������Q�[���I�u�W�F�b�g���܂Ƃ߂�1�̃I�u�W�F�b�g�Ƃ��Ĉ����N���X
        /// </summary>
        public class PoolObject
        {
            public List<GameObject> poolObjects;

            public PoolObject()
            {
                poolObjects = new List<GameObject>();
            }

            public bool ActiveSelf
            {
                get
                {
                    foreach (var item in poolObjects)
                    {
                        if (item.activeSelf) return true;
                    }
                    return false;
                }
            }


            public void AddObj(GameObject obj)
            {
                poolObjects.Add(obj);
            }

            public void SetActiveAll(bool value)
            {
                foreach (var item in poolObjects)
                {
                    item.SetActive(value);
                }
            }

            public void SetActive(GameObject obj, bool value)
            {
                foreach (var item in poolObjects)
                {
                    if (item.name == obj.name + "(Clone)")
                    {
                        item.SetActive(value);
                        return;
                    }
                }
            }

            public void SetPositionAndRotation(Vector3 pos, Quaternion quaternion)
            {
                foreach (var item in poolObjects)
                {
                    item.transform.position = pos;
                    item.transform.rotation = quaternion;
                }
            }

        }

        public List<PoolObject> poolTable;

        /// <summary>
        /// �I�u�W�F�b�g�v�[���̃e�[�u����������
        /// </summary>
        public PoolObjectManager()
        {
            poolTable = new List<PoolObject>();
        }

        /// <summary>
        /// �V�����v�[���I�u�W�F�b�g�𐶐����A�Ԃ��B
        /// �v�[���e�[�u���ɒǉ�����B  
        /// �Ăяo�����ŁA�v�[�����镪�����Ăяo���B
        /// </summary>
        /// <returns></returns>
        public PoolObject InstantiatePoolObj()
        {
            var poolObject = new PoolObject();
            poolTable.Add(poolObject);
            return poolObject;
        }


        /// <summary>
        /// �v�[���I�u�W�F�b�g��S�Ďg�p����
        /// </summary>
        public void UsePoolObject(Vector3 pos, Quaternion quaternion, Func<PoolObject> func = null)
        {
            foreach (var item in poolTable)
            {
                if (item.ActiveSelf) continue;

                item.SetPositionAndRotation(pos, quaternion);
                item.SetActiveAll(true);
                return;
            }

            if (func == null) return;

            //�v�[���I�u�W�F�b�g�̑S�ẴA�N�e�B�u��true�Ȃ�
            //�V�����v�[���I�u�W�F�b�g�𐶐�����
            var newPoolObj = func.Invoke();
            newPoolObj.SetPositionAndRotation(pos, quaternion);
            newPoolObj.SetActiveAll(true);
        }

        /// <summary>
        /// �w�肵���Q�[���I�u�W�F�b�g���g�p����
        /// </summary>
        public void UsePoolObject(GameObject gameObject, Vector3 pos, Quaternion quaternion, Func<PoolObject> func = null)
        {
            foreach (var item in poolTable)
            {
                if (item.ActiveSelf) continue;

                item.SetPositionAndRotation(pos, quaternion);
                item.SetActive(gameObject, true);
                return;
            }

            if (func == null) return;

            //�v�[���I�u�W�F�b�g�̑S�ẴA�N�e�B�u��true�Ȃ�
            //�V�����v�[���I�u�W�F�b�g�𐶐�����
            var newPoolObj = func.Invoke();
            newPoolObj.SetPositionAndRotation(pos, quaternion);
            newPoolObj.SetActive(gameObject, true);
        }
    }
}
