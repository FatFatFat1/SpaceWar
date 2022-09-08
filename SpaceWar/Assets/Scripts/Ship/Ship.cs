using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 pointOfDepartures;

    private float startTime;
    private float speed = 0.5f;
    public void Fly(Vector3 fromPlanet, GameObject victim)
    {
        target = victim;
        pointOfDepartures = fromPlanet;
    }
    private void Start()
    {
        startTime = Time.time;
    }
    private void Update()
    {
        float distCovered = Time.time - startTime;
        transform.position = Vector3.Lerp(pointOfDepartures, target.transform.position, distCovered* speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            Destroy(gameObject);
            target.GetComponent<Planet>().Ships++; //сел
        }
    }
}
