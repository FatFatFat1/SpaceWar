using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FactionData;

public class Planet : MonoBehaviour
{
    private const int _shipForDay = 5; // Сколько кораблей строиться за "день"
    private TMPro.TextMeshProUGUI _text;
    private GameObject _gameController;

    [SerializeField] private Faction _myFaction;
    [SerializeField] private GameObject objToText;
    [SerializeField] private GameObject _selectedPointer;
    [SerializeField] private GameObject _shipPrefab;

    public GameObject SelectedPointer => _selectedPointer;
    public bool IsSelected = false;
    public int Ships = 0; // Сколько кораблей на планете
    private void Start()
    {
        _text = objToText.GetComponent<TMPro.TextMeshProUGUI>();
        _gameController = GameObject.FindGameObjectWithTag("Controller");
        StartCoroutine(ShipBuilding());
        GetStartFaction();
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
            Ships += _shipForDay;
            _text.text = Ships.ToString();
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
        GameObject planetToFly = _gameController.GetComponent<SelectController>().currentSelected;
        if (planetFrom != null)
        {
            Departure(planetFrom, planetToFly);
            planetFrom.GetComponent<Planet>().Ships = (int) (Ships/2);
            _gameController.GetComponent<SelectController>().EndTurnReset();
        }
    }

    void GetStartFaction()
    {
        GetFaction("Neutral");// Изначально все планеты никому не принадлежат
        
    }

    void Departure(GameObject fromPlanet, GameObject victim)
    {
        int ship_to_fly = (int)(Ships/2); //Половина кораблей вылетает
        Vector3 pos = new Vector3(fromPlanet.transform.position.x, fromPlanet.transform.position.y + 0.5f, fromPlanet.transform.position.z);
        for (int i = 0; i < ship_to_fly; i++)
        {
            GameObject newShip = Instantiate(_shipPrefab, pos, Quaternion.identity);
            pos = new Vector3(pos.x, pos.y + 0.2f, pos.z);
            if ((i + 1) % 3 == 0)
            {
                pos = new Vector3(pos.x - 0.2f , pos.y - 0.6f, pos.z);
            }
            newShip.GetComponent<Ship>().Fly(pos, victim);
        }

    }

    void GetFaction(string name)
    {
        for (int i = 1; i < _gameController.GetComponent<FactionData>()._faction.Length; i++)
        {
            if (name == _gameController.GetComponent<FactionData>()._faction[i].Name)
            {
                _myFaction = _gameController.GetComponent<FactionData>()._faction[i];
                gameObject.GetComponent<SpriteRenderer>().color = _gameController.GetComponent<FactionData>()._faction[i].Color;
            }
        }
    }
}

