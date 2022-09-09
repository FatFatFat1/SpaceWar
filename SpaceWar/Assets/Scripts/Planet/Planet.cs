using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FactionData;

public class Planet : MonoBehaviour
{
    private const int _shipForDay = 5; // Сколько кораблей строиться за "день"

    [SerializeField] private GameObject _gameController;
    [SerializeField] private int _maxShips;
    [SerializeField] private Faction _myFaction;
    [SerializeField] private GameObject objToText;
    [SerializeField] private GameObject _selectedPointer;
    [SerializeField] private GameObject _shipPrefab;

    public GameObject SelectedPointer => _selectedPointer;
    public Faction MyFaction => _myFaction;
    private float _radius;
    public float Radius => _radius;
    public bool IsSelected = false;
    public int Ships; // Сколько кораблей на планете
    public TMPro.TextMeshProUGUI Text;
    public bool NotForCheck = false;

    private void Awake()
    {
        _radius = GetComponent<CircleCollider2D>().radius;
        _gameController = GameObject.FindGameObjectWithTag("Controller");
        Text = objToText.GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Start()
    {
        if (_myFaction.Name == "")
        {
            GetStartFaction();
            Ships = Random.Range(1, _maxShips);
            Text.text = Ships.ToString();
        }
        else
        {
            StartCoroutine(ShipBuilding()); //Только занятые планеты увеличивают популяцию
        }
        GetFactionColor();
    }

    private void Update()
    {
        if (IsSelected)
        {
            GetComponent<LineRenderer>().enabled = true;
            _gameController.GetComponent<SelectController>().LineCreate();
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(ShipBuilding()); // Планеты не должны пропадать во время игры , но а вдруг?
    }
    IEnumerator ShipBuilding() //Прирост кораблей
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (Ships <= _maxShips)
            {
                if (_maxShips - Ships > _shipForDay)
                {
                    Ships += _shipForDay;
                }
                else
                {
                    Ships = _maxShips;
                }
            }
            Text.text = Ships.ToString();
        }
    }
    public void SetSelected()
    {
        _gameController.GetComponent<SelectController>().SwitchSelected(gameObject);
    }
    private void OnMouseDown()
    {
        GameObject planetFrom = _gameController.GetComponent<SelectController>().oldSelecred;
        SetSelected();
        GameObject planetTo = _gameController.GetComponent<SelectController>().currentSelected;
        if (planetFrom != null)
        {
            if (planetFrom.GetComponent<Planet>()._myFaction.Name == _gameController.GetComponent<SelectController>().Owner) // Посылать можно только со своих планет
            {
                Departure(planetFrom, planetTo);
                planetFrom.GetComponent<Planet>().Ships = Mathf.FloorToInt(planetFrom.GetComponent<Planet>().Ships / 2);
                planetFrom.GetComponent<Planet>().Text.text = planetFrom.GetComponent<Planet>().Ships.ToString();
                _gameController.GetComponent<SelectController>().EndTurnReset();
            }
            else
            {
                _gameController.GetComponent<SelectController>().EndTurnReset();
            }
        }
    }

    void Departure(GameObject fromPlanet, GameObject victim)
    {
        int ship_to_fly = Mathf.FloorToInt(fromPlanet.GetComponent<Planet>().Ships / 2); //Половина кораблей вылетает
        Vector3 pos = new Vector3(fromPlanet.transform.position.x, fromPlanet.transform.position.y + 0.5f, fromPlanet.transform.position.z);
        for (int i = 0; i < ship_to_fly; i++)
        {
            GameObject newShip = Instantiate(_shipPrefab, pos, Quaternion.identity);
            pos = new Vector3(pos.x, pos.y + 0.2f, pos.z);
            if ((i + 1) % 3 == 0)
            {
                pos = new Vector3(pos.x - 0.2f, pos.y - 0.6f, pos.z);
            }
            newShip.GetComponent<Ship>().Fly(fromPlanet, pos, victim);
        }

    }


    void GetStartFaction()
    {
        GetFaction("Neutral");// Изначально все планеты никому не принадлежат
        GetFactionColor();
    }

    public void GetStartPlayerFaction()
    {
        GetFaction("Player");
        GetFactionColor();
        Ships = 50;
        Text.text = Ships.ToString();

    }

    void GetFaction(string name)
    {
        for (int i = 0; i < _gameController.GetComponent<FactionData>()._faction.Length; i++)
        {
            if (name == _gameController.GetComponent<FactionData>()._faction[i].Name)
            {
                _myFaction = _gameController.GetComponent<FactionData>()._faction[i];
            }
        }
    }

    void GetFactionColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = _myFaction.Color;
    }

    public void Capture(string newOwner)
    {
        if (MyFaction.Name == "Neutral")
        {
            StartCoroutine(ShipBuilding());//Захват нейтральной планеты запускает рост популяции
        }
        GetFaction(newOwner);
        GetFactionColor();
    }
}