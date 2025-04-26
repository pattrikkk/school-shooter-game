using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject projectile;
    public float power = 20.0f;
    // sila/rýchlosť výstrelu    
    public GameObject shootPoint;
    // pozícia na ktorej vznikne projektil  
    public GameObject grabPoint;
    // pozícia na ktorej vznikne projektil      
    public void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, shootPoint.transform.position, shootPoint.transform.rotation) as GameObject;
        newProjectile.GetComponent<Rigidbody>().AddForce(grabPoint.transform.forward * power, ForceMode.VelocityChange);
        ShootRay();
    }

    private void ShootRay()
    {
        Ray ray = new Ray(shootPoint.transform.position, shootPoint.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20f))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {

                hit.collider.GetComponentInParent<EnemyAI>().TakeDamage(1);
            }
        }
    }
}
