using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ClassroomSpawnArea : MonoBehaviour
{
    [Range(0, 6)]
    public int maxStudents = 6;


    // Zobraziť Areu kde sa môže spawnúť čávo


    /*     private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.3f);
            Gizmos.DrawCube(transform.position, transform.localScale);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, transform.localScale);

            BoxCollider collider = GetComponent<BoxCollider>();
            if (collider != null)
            {
                Collider[] hitColliders = Physics.OverlapBox(
                    collider.bounds.center,
                    collider.bounds.extents,
                    Quaternion.identity
                );

                Gizmos.color = Color.black;
                foreach (var hitCollider in hitColliders)
                {
                    string objectName = hitCollider.gameObject.name.ToLower();
                    if (hitCollider.gameObject != gameObject &&
                        !objectName.Contains("floor"))
                    {
                        Bounds objectBounds = hitCollider.bounds;

                        Gizmos.DrawCube(objectBounds.center, objectBounds.size);
                    }
                }
            }
        } */
}