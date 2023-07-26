using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public int Damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;

    float timeToFire = 0;
    Transform firePoint;

    // Start is called before the first frame update
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("There is no firepoint!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }

        }
    }
    //Shooting Raycasting
    void Shoot()
    {
        //Screen co ordinates to world
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        // Taking the firepoint and storing it as a position
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        //Raycasting in a direction with origin and direction
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        // Changes made for the bullet trail
        Effect();

        //Making the ray continue off the screen by making it a lot bigger
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 200, Color.blue);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);

            
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy!= null)
            {
                enemy.DamageEnemy(Damage);
                //Saying what we hit and with what damage
                Debug.Log("Hitting " + hit.collider.name + " with damage: " + Damage);
            }
        }

    }
    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
    }
}


