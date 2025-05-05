using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float _requiredVisitTime = 3f;
    [SerializeField] private PlayerVR _playerVR; 
    private List<ClassroomSpawnArea> _allClassrooms = new List<ClassroomSpawnArea>();
    private int _visitedCount;
    private Dictionary<ClassroomSpawnArea, float> _playerEnterTimes = new Dictionary<ClassroomSpawnArea, float>();

    // Quest state tracking
    private bool _enemyNeutralized = false;
    private bool _allClassroomsVisited = false;
    private bool _allNpcsSaved = false;

    // Events
    public event Action<ClassroomSpawnArea> OnClassroomVisited;

    void OnEnable()
    {
        InteractableNPC.OnAllRequiredNpcsSaved += HandleNpcsSaved;
        EnemyAI.OnEnemyNeutralized += HandleEnemyNeutralized;
    }

    void OnDisable()
    {
        InteractableNPC.OnAllRequiredNpcsSaved -= HandleNpcsSaved;
        EnemyAI.OnEnemyNeutralized -= HandleEnemyNeutralized;
    }

    private void HandleNpcsSaved(bool isCompleted)
    {
        Debug.Log("All required NPCs have been saved!");
        _allNpcsSaved = isCompleted;
        CheckAllQuestsCompletion();
    }

    private void HandleEnemyNeutralized(bool isCompleted)
    {
        Debug.Log("Enemy has been neutralized!");
        _enemyNeutralized = isCompleted;
        CheckAllQuestsCompletion();
    }

    private void HandleClassroomQuest(bool isCompleted)
    {
        Debug.Log("All classrooms have been visited!");
        _allClassroomsVisited = isCompleted;
        CheckAllQuestsCompletion();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterClassroom(ClassroomSpawnArea classroom)
    {
        if (!_allClassrooms.Contains(classroom))
        {
            _allClassrooms.Add(classroom);
            Debug.Log($"Registered classroom: {classroom.name}");
        }
    }

    public void HandlePlayerEnter(ClassroomSpawnArea classroom)
    {
        if (!_playerEnterTimes.ContainsKey(classroom))
        {
            _playerEnterTimes[classroom] = Time.time;
            Debug.Log($"Player entered {classroom.name} at {Time.time}");
        }
    }

    public void HandlePlayerExit(ClassroomSpawnArea classroom)
    {
        if (_playerEnterTimes.ContainsKey(classroom))
        {
            _playerEnterTimes.Remove(classroom);
        }
    }

    void Update()
    {
        List<ClassroomSpawnArea> toRemove = new List<ClassroomSpawnArea>();
        float currentTime = Time.time;

        foreach (var kvp in _playerEnterTimes)
        {
            ClassroomSpawnArea classroom = kvp.Key;
            float enterTime = kvp.Value;

            if (currentTime - enterTime >= _requiredVisitTime && !classroom.HasBeenVisited)
            {
                classroom.MarkAsVisited();
                _visitedCount++;
                Debug.Log($"Classroom {classroom.name} visited! Total visited: {_visitedCount}/{_allClassrooms.Count}");

                if (OnClassroomVisited != null)
                {
                    OnClassroomVisited.Invoke(classroom);
                }

                toRemove.Add(classroom);
                CheckCompletion();
            }
        }

        foreach (var classroom in toRemove)
        {
            _playerEnterTimes.Remove(classroom);
        }
    }

    private void CheckCompletion()
    {
        if (_visitedCount >= _allClassrooms.Count && !_allClassroomsVisited)
        {
            _allClassroomsVisited = true;

            HandleClassroomQuest(true);
        }
    }

    private void CheckAllQuestsCompletion()
    {
        if (_enemyNeutralized && _allClassroomsVisited && _allNpcsSaved)
        {
            _playerVR.ShowMessage("GAME COMPLETED!\n All three quests have been finished.\n New level loading!", false);
            Debug.Log("GAME COMPLETED! All three quests have been finished.");
            
        }
        else
        {
            _playerVR.ShowMessage($"Quest progress: Classrooms: {_allClassroomsVisited}, Enemy: {_enemyNeutralized}, NPCs: {_allNpcsSaved}", true);
            Debug.Log($"Quest progress: Classrooms: {_allClassroomsVisited}, Enemy: {_enemyNeutralized}, NPCs: {_allNpcsSaved}");
        }
    }
}