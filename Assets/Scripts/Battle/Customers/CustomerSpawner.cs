using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] CustomerSetUpData data;
    [SerializeField] Transform customerParent;
    [SerializeField] Transform noteParent;
    [SerializeField] CustomerView customerViewPrefab;
    [SerializeField] NoteView noteViewPrefab;
}
