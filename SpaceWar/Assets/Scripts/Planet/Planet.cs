using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private int _ships = 0; // ������� �������� �� �������
    private Faction _faction; // ���� ����������� �������
    private const int _shipForDay = 5; // ������� �������� ��������� �� "����"
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
        StopCoroutine(ShipBuilding()); // ������� �� ������ ��������� �� ����� ���� , �� � �����?
    }
    IEnumerator ShipBuilding() //������� ��������
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

