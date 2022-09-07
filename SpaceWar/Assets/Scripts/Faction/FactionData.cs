using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionData : MonoBehaviour
{
    public Faction[] _faction;

    [System.Serializable]
    public class Faction
    {
        public string name;
        public Color color;
    }
}

