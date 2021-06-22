using System.Collections;
using System.Collections.Generic;

namespace Musahi.MY_VR_Games
{
    /// <summary>
    /// ダメージ処理をするクラスが継承するインターファイス
    /// </summary>
    public interface IDamagable
    {
        /// <summary>
        /// ダメージ処理
        /// </summary>
        /// <param name="damage"></param>
        void OnDamage(float damage = 0f);
    }
}