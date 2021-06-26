using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Musahi.MY_VR_Games.DualWield
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float lifeTime = 1f;
        [SerializeField] float shotPower = 100f;

        bool init = true;
        Rigidbody rb;
        Vector3 prevPos;

        private void OnEnable()
        {
            if (!rb)
            {
                rb = GetComponent<Rigidbody>();
            }
            prevPos = transform.position;
            //rb.velocity = transform.forward * shotPower;
            gameObject.DelaySetActive(false, lifeTime, this.GetCancellationTokenOnDestroy()).Forget();
            //if (init == false)
            //{
            //    gameObject.DelaySetActive(false, lifeTime, this.GetCancellationTokenOnDestroy()).Forget();
            //}
            //init = false;
        }


        public void SetVelocity(Transform muzzle)
        {
            transform.forward = muzzle.forward;
            rb.velocity = muzzle.forward * shotPower;
        }

        private void Update()
        {
            RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

            for (int i = 0; i < hits.Length;)
            {
                if (hits[i].collider.TryGetComponent(out IDamagable target))
                {
                    target.OnDamage();
                }
                gameObject.SetActive(false);
                break;
            }
            prevPos = transform.position;
        }

    }
}