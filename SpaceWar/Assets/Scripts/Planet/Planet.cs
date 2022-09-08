using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FactionData;

public class Planet : MonoBehaviour
{
    private int _ships = 0; // Сколько кораблей на планете
    private const int _shipForDay = 5; // Сколько кораблей строиться за "день"
    private TMPro.TextMeshProUGUI _text;
    private GameObject _gameController;

    [SerializeField] private Faction _myFaction;
    [SerializeField] private GameObject objToText;
    [SerializeField] private GameObject _selectedPointer;

    public GameObject SelectedPointer => _selectedPointer;
    public bool IsSelected = false;
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
            _ships += _shipForDay;
            _text.text = _ships.ToString();
        }
    }
    public void SetSelected()
    {
        _gameController.GetComponent<SelectController>().SwitchSelected(gameObject);
    }
    private void OnMouseDown()
    {
        SetSelected();
    }

    void GetStartFaction()
    {
        GetFaction("Neutral");// Изначально все планеты никому не принадлежат
    }


    void GetFaction(string name)
    {
        for (int i = 1; i < _gameController.GetComponent<FactionData>()._faction.Length; i++)
        {
            if (name == _gameController.GetComponent<FactionData>()._faction[i].name)
            {
                _myFaction = _gameController.GetComponent<FactionData>()._faction[i];
                gameObject.GetComponent<SpriteRenderer>().color = _gameController.GetComponent<FactionData>()._faction[i].color;
            }
        }
    }
}

