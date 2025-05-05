using UnityEngine;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ClassroomSpawnArea : MonoBehaviour
{
    public int maxStudents = 3;
    public bool HasBeenVisited { get; private set; }

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GameManager.Instance.RegisterClassroom(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.HandlePlayerEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.HandlePlayerExit(this);
        }
    }

    public void MarkAsVisited()
    {
        HasBeenVisited = true;
    }


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