using UnityEngine;
using System.Collections.Generic;

public class MachineGun : MonoBehaviour
{
    public float range = 10f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    private List<Transform> enemiesInRange = new List<Transform>();

    void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = range;
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }

    void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            Transform target = enemiesInRange[0];
            AimAndShoot(target);
        }
    }

    void AimAndShoot(Transform target)
    {

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);


        if (fireCountdown <= 0f)
        {
            Shoot(target);
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot(Transform target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {

            bullet.Seek(target);
            Debug.Log("Bullet fired towards: " + target.name);
        }
        else
        {
            Debug.Log("Dan khong duoc tim thay");
        }
    }
}