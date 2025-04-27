using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerControllerNonVR _playerNonVR;
    private List<GameObject> allStudentPrefabs;
    private GameObject shooterPrefab;
    private List<GameObject> regularStudentPrefabs;
    private List<ClassroomSpawnArea> classrooms = new List<ClassroomSpawnArea>();
    private float spawnClearanceRadius = 2f;
    [SerializeField] Transform _safePlace;

    public LayerMask collisionCheckLayers;

    public event Action<string> OnShooterSpawned;

    void Start()
    {
        loadResources();
        SpawnEntitiesInClassrooms();
    }

    void loadResources()
    {
        allStudentPrefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Students"));
        if (allStudentPrefabs.Count == 0) Debug.LogError("No student prefabs found in Resources/Students!");

        GameObject[] attackers = Resources.LoadAll<GameObject>("Attackers");
        if (attackers.Length == 0) Debug.LogError("No attacker prefabs found in Resources/Attackers!");
        shooterPrefab = attackers[UnityEngine.Random.Range(0, attackers.Length)];

        classrooms.AddRange(FindObjectsOfType<ClassroomSpawnArea>());
        if (classrooms.Count == 0) Debug.LogError("No ClassroomSpawnArea components found in the scene!");

    }

    void SpawnEntitiesInClassrooms()
    {
        if (classrooms.Count == 0) return;

        ClassroomSpawnArea shooterRoom = classrooms[UnityEngine.Random.Range(0, classrooms.Count)];
        var shooterSpawned = TrySpawnEntity(shooterPrefab, shooterRoom);

        if (!shooterSpawned)
        {
            shooterRoom = classrooms[UnityEngine.Random.Range(0, classrooms.Count)];
            shooterSpawned = TrySpawnEntity(shooterPrefab, shooterRoom);
        }

        var enemyAI = shooterSpawned.GetComponent<EnemyAI>();
        enemyAI.Setup(_playerNonVR.transform);
        Debug.Log($"Shooter spawned in {enemyAI.enemyDescription}");
        OnShooterSpawned?.Invoke(enemyAI.enemyDescription);

        foreach (var classroom in classrooms)
        {
            int studentsToSpawn = UnityEngine.Random.Range(0, classroom.maxStudents + 1);
            for (int i = 0; i < studentsToSpawn; i++)
            {
                GameObject randomStudent = allStudentPrefabs[UnityEngine.Random.Range(0, allStudentPrefabs.Count)];
                var spawnedStudent = TrySpawnEntity(randomStudent, classroom);
                spawnedStudent.GetComponent<InteractableNPC>().Setup(_safePlace);
            }
        }
    }

    GameObject TrySpawnEntity(GameObject prefab, ClassroomSpawnArea classroom)
    {
        BoxCollider classroomCollider = classroom.GetComponent<BoxCollider>();
        if (classroomCollider == null) return null;

        for (int i = 0; i < 30; i++)
        {
            Vector3 spawnPos = GetRandomPositionInBounds(classroom);
            Vector3 spawnAreaSize = new Vector3(0.5f, 1f, 0.5f);

            if (!Physics.CheckBox(spawnPos, spawnAreaSize / 2, Quaternion.identity, collisionCheckLayers))
            {
                var obj = Instantiate(prefab, spawnPos, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
                return obj;
            }
        }
        return null;
    }

    Vector3 GetRandomPositionInBounds(ClassroomSpawnArea classroom)
    {
        BoxCollider collider = classroom.GetComponent<BoxCollider>();
        Bounds bounds = collider.bounds;

        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x + spawnClearanceRadius, bounds.max.x - spawnClearanceRadius),
            0.85f,
            UnityEngine.Random.Range(bounds.min.z + spawnClearanceRadius, bounds.max.z - spawnClearanceRadius)
        );
    }
}