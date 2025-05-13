using UnityEngine;
using UnityEngine.VFX;


public class GunScript : MonoBehaviour
{
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] VisualEffect muzzleFlash;
    public GameObject projectile;
    public GameObject bulletHole;
    public float power = 20.0f;
    // sila/rýchlosť výstrelu    
    public GameObject shootPoint;
    // pozícia na ktorej vznikne projektil  
    public GameObject grabPoint;
    // pozícia na ktorej vznikne projektil      
    private bool _isEnemy = false;
    public void Shoot(bool isEnemy = false)
    {
        _isEnemy = isEnemy;
        GameObject newProjectile = Instantiate(projectile, shootPoint.transform.position, shootPoint.transform.rotation) as GameObject;
        newProjectile.GetComponent<Rigidbody>().AddForce(grabPoint.transform.forward * power, ForceMode.VelocityChange);
        ShootRay();
        muzzleFlash.Play();
        if (_audioSource && _shootSound)
        {
            _audioSource.PlayOneShot(_shootSound);
        }
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
            
            if (hit.collider.CompareTag("Player"))
            {

                hit.collider.GetComponentInParent<PlayerVR>().TakeDamage(1);
            }
            GameObject newBulletHole = Instantiate(bulletHole, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(-hit.normal));
        }
    }
}
