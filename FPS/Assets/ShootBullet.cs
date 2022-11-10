using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBullet : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera gun;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage); 
            }
        }

    }
}
