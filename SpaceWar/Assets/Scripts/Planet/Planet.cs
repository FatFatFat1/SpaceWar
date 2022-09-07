using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private int _ships = 0; // Сколько кораблей на планете
    private Faction _faction; // Кому принадлежит планета
    private const int _shipForDay = 5; // Сколько кораблей строиться за "день"
    private TMPro.TextMeshProUGUI _text;
    private GameObject _gameController;

    [SerializeField] private GameObject objToText;
    [SerializeField] private GameObject _selectedPointer;

    public GameObject SelectedPointer => _selectedPointer;
    public bool IsSelected = false;
    private void Start()
    {
        _text = objToText.GetComponent<TMPro.TextMeshProUGUI>();
        _gameController = GameObject.FindGameObjectWithTag("Controller");
        StartCoroutine(ShipBuilding());
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
}

