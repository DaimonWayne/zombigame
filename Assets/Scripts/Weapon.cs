using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Weapon : MonoBehaviour
{
    public GunType weaponProperties;
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float nextTimeToFire = 0f;
    [SerializeField] private int currentAmmo = 0;

    private void Awake()
    {
        currentAmmo = weaponProperties._bulletCapacity;
    }

    public void FireRequest()
    {
        if (currentAmmo > 0)
        {
            if (weaponProperties._ammunition == GunType.Ammunition.bullet)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / weaponProperties.fireRate;
                    ShootBullet();
                }
            }
            else if (weaponProperties._ammunition == GunType.Ammunition.flare)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / weaponProperties.fireRate;
                    ShootFlare();
                }
            }
        }
        else
        {
            Debug.Log("Mermi Bitti");
        }

    }

    // Riffle and pistols
    void ShootBullet()
    {
        muzzleFlash.Play();
        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, weaponProperties._range))
        {
            Debug.Log(hit.transform.name);

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);

            EnemyBehaviour enemy = hit.transform.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponProperties._damage);
            }
        }
    }

    // Shotgun
    private void ShootFlare()
    {
        weaponProperties.fireRate = 2.5f;
        muzzleFlash.Play();
        currentAmmo--;

        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, weaponProperties._range))
        {
            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + new Vector3(-0.1f, 0f, 0f), out hit2, weaponProperties._range))
        {
            GameObject impactGo = Instantiate(impactEffect, hit2.point, Quaternion.LookRotation(hit2.normal));
            Destroy(impactGo, 2f);
        }

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + new Vector3(0f, -0.2f, 0f), out hit3, weaponProperties._range))
        {
            GameObject impactGo = Instantiate(impactEffect, hit3.point, Quaternion.LookRotation(hit3.normal));
            Destroy(impactGo, 2f);
        }

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + new Vector3(0f, 0f, 0.1f), out hit4, weaponProperties._range))
        {
            GameObject impactGo = Instantiate(impactEffect, hit4.point, Quaternion.LookRotation(hit4.normal));
            Destroy(impactGo, 2f);
        }
    }

   

}
