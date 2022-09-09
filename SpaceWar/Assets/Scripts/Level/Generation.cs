using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [SerializeField] private Vector3 _borderCoordinate;
    [SerializeField] private GameObject _planetPrefab;
    [SerializeField] private int _maxPlanetCount;

    private float _sumRadius;
    private GameObject _planet = null;
    private bool isFirst = true;
    private void Start()
    {
        for (int i = 0; i <= _maxPlanetCount; i++)
        {
            if (_planet != null)
            {
                _planet = TryCreatePlanet(_sumRadius, _planet);
            }
            if (isFirst)
            {
                _planet = CreatePlanet(new Vector3(Random.Range(-_borderCoordinate.x, _borderCoordinate.x), Random.Range(-_borderCoordinate.y, _borderCoordinate.y), 0));
                _sumRadius = _planet.GetComponent<Planet>().Radius;
                isFirst = false;
            }
        }
        GameObject[] allPlanet = GameObject.FindGameObjectsWithTag("Planet");
        int rand = Mathf.FloorToInt(Random.Range(0, allPlanet.Length));
        allPlanet[rand].GetComponent<Planet>().GetStartPlayerFaction();

    }

    GameObject TryCreatePlanet(float sumRadius, GameObject obj)
    {
        GameObject planet;
        GameObject[] allPlanet = GameObject.FindGameObjectsWithTag("Planet");
        obj.GetComponent<CircleCollider2D>().enabled = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(obj.transform.position, sumRadius + obj.GetComponent<Planet>().Radius);
        if (CheckRadius(hits)) //Рядом ничего нет
        {
            Vector3 pos = obj.transform.position + 2 * new Vector3(Random.Range(-sumRadius, sumRadius) <= 0 ? -sumRadius : sumRadius, Random.Range(-sumRadius, sumRadius) <= 0 ? -sumRadius : sumRadius, 0);
            for (int x = 0; x < allPlanet.Length; x++)
            {
                if (pos != allPlanet[x].transform.position)
                {
                    allPlanet = GameObject.FindGameObjectsWithTag("Planet");
                    for (int i = 0; i < allPlanet.Length; i++)
                    {
                        allPlanet[i].GetComponent<Planet>().NotForCheck = false;
                    }
                }
            }
            sumRadius = obj.gameObject.GetComponent<Planet>().Radius;
            _sumRadius = sumRadius;
            planet = CreatePlanet(pos);
            obj.GetComponent<CircleCollider2D>().enabled = true;
            return planet;
        }
        else
        {
            foreach (Collider2D c in hits)
            {
                sumRadius += c.gameObject.GetComponent<Planet>().Radius; //Сумма радиусов соседних планет
                _sumRadius = sumRadius;
                c.gameObject.GetComponent<Planet>().NotForCheck = true;
                obj.GetComponent<CircleCollider2D>().enabled = true;
            }
            obj.GetComponent<CircleCollider2D>().enabled = true;
        }
        planet = obj;
        return planet;
    }
    bool CheckRadius(Collider2D[] hits)
    {
        foreach (Collider2D c in hits)
        {
            if (c.gameObject.CompareTag("Planet"))
            {
                if (c.gameObject.GetComponent<Planet>().NotForCheck == false)
                {
                    return false;
                }
            }

        }
        return true;
    }
    GameObject CreatePlanet(Vector3 pos)
    {
        GameObject planet = Instantiate(_planetPrefab, pos, Quaternion.identity);
        return planet;
    }
}
