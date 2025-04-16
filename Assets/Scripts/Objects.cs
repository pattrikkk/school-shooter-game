using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "All Objects", menuName = "ScriptableObjects/Objects")]
public class Objects : ScriptableObject
{
    public List<GameObject> Students;
    public List<GameObject> Shooters;
    public GameObject Gun;
}
