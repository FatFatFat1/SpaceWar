using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionData : MonoBehaviour
{
    public Faction[] _faction;

    [System.Serializable]
    public class Faction
    {
        [SerializeField] private string name;
        [SerializeField] private Color color;
        public string Name => name;
        public Color Color => color;
    }
}

