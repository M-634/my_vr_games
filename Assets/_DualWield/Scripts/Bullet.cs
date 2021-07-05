using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Musahi.MY_VR_Games.DualWield
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float lifeTime = 1f;
        [SerializeField] float shotPower = 100f;
        [SerializeField] float correctionDistance = 0.1f;

        Rigidbody rb;
        Vector3 prevPos;

        private void OnEnable()
        {
            if (!rb)
            {
                rb = GetComponent<Rigidbody>();
            }
            var target = Camera.main.transform.position + Vector3.forward * correctionDistance;
            var dir = (target - transform.position).normalized;
            rb.velocity = dir * shotPower;
            prevPos = transform.position;
            gameObject.DelaySetActive(false, lifeTime, this.GetCancellationTokenOnDestroy()).Forget();
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