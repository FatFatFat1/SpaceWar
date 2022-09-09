using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 pointOfDepartures;

    private GameObject _myPlanet;
    private Rigidbody2D rg;
    private float startTime;
    private float speed = 0.5f;

    public void Fly(GameObject myPlanet, Vector3 pos, GameObject victim)
    {
        target = victim;
        pointOfDepartures = pos;
        _myPlanet = myPlanet;
    }
    private void Start()
    {
        transform.position = pointOfDepartures;
        startTime = Time.time;
        Color myColor = _myPlanet.GetComponent<Planet>().MyFaction.Color;
        gameObject.GetComponent<SpriteRenderer>().color = myColor;
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float distCovered = Time.time - startTime;
        rg.AddForce(target.transform.position - transform.position);
        rg.position = Vector3.Lerp(pointOfDepartures, target.transform.position, distCovered * speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            Destroy(gameObject);
            Planet planet = target.GetComponent<Planet>();
            if (planet.MyFaction.Name == _myPlanet.GetComponent<Planet>().MyFaction.Name) // Можно пополнять "запасы" своих же планет
            {
                planet.Ships++;
            }
            else //Если планета не твоей фракции , то корабли умирают для захвата
            {
                if (planet.Ships > 0)
                {
                    planet.Ships--;
                }
                if (planet.Ships == 0) //Корабль влетает в "незанятую" планету
                {

                    planet.Capture(_myPlanet.GetComponent<Planet>().MyFaction.Name);
                }
            }
            planet.Text.text = planet.Ships.ToString();
        }
    }
}
