using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Shooting Properties")]
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public Camera fpsCam = null;
    public ParticleSystem muzzleFlash = null;
    public AudioSource gunShot = null;
    public GameObject impactEffect = null;
    private float nextTimeToFire = 0f;

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            gunShot.Play();
            muzzleFlash.Play();

            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name.ToString());
                Target target = hit.transform.GetComponent<Target>();

                if (target)
                {
                    target.TakeDamage(damage);
                }
            }

            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce,ForceMode.Impulse);
            }

            nextTimeToFire = Time.time + 1f / fireRate;

            GameObject particle = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(particle, 0.2f);
        }
    }
}
