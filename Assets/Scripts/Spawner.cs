using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Objects _allSpawnableObjects;
    [SerializeField] Transform _player;
    [SerializeField] PlayerControllerNonVR _playerNonVR;
    //pridat pozicie z ktorych sa random vyberu pre studentov a potom dalsi list pre ENEMY

    private void Start()
    {
        SpawnAI();
        SpawnStudents();
    }

    private void SpawnStudents()
    {
        foreach (var student in _allSpawnableObjects.Students)
        {

        }
    }

    void SpawnAI()
    {
        var shooterPrefab = _allSpawnableObjects.Shooters[0]; // random dat
        Vector3 spawnPosition = new Vector3(-2.76999998f, 1.09f, -0.419999987f); // dat to nahodne

        GameObject spawnedShooter = Instantiate(shooterPrefab, spawnPosition, Quaternion.identity);
        var aiScript = spawnedShooter.GetComponent<EnemyAI>();
        aiScript.Setup(_playerNonVR.transform);

        // choose random typek from _allSpawnableObjects, getComponent EnemyAI, zabolaù public metodu na setup, kde si poöleö referenciu na hr·Ëa  
    }
}
