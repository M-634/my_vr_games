using System.Collections;
using System.Collections.Generic;

namespace Musahi.MY_VR_Games
{
    /// <summary>
    /// �_���[�W����������N���X���p������C���^�[�t�@�C�X
    /// </summary>
    public interface IDamagable
    {
        /// <summary>
        /// �_���[�W����
        /// </summary>
        /// <param name="damage"></param>
        void OnDamage(float damage = 0f);
    }
}