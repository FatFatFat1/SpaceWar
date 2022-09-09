using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour
{
    [SerializeField] private string _owner;

    private GameObject _oldSelected;
    private GameObject _currentSelected;
    public GameObject currentSelected => _currentSelected;
    public string Owner => _owner; // По сути - игрок

    public GameObject oldSelecred => _oldSelected;

    public void SwitchSelected(GameObject currentSelected)
    {
        if (_currentSelected != currentSelected) //нажал по уже выделеному
        {
            currentSelected.GetComponent<Planet>().IsSelected = !currentSelected.GetComponent<Planet>().IsSelected;
            currentSelected.GetComponent<Planet>().SelectedPointer.SetActive(currentSelected.GetComponent<Planet>().IsSelected);
            _currentSelected = currentSelected;
        }
        if (_oldSelected == null)
        {
            _oldSelected = _currentSelected;
        }
        else if (_oldSelected != _currentSelected)
        {
            _oldSelected.GetComponent<Planet>().IsSelected = !_oldSelected.GetComponent<Planet>().IsSelected;
            _oldSelected.GetComponent<Planet>().SelectedPointer.SetActive(_oldSelected.GetComponent<Planet>().IsSelected);
            _oldSelected = _currentSelected;
        }
    }

    public void LineCreate()
    {
        LineRenderer lineRenderer = _currentSelected.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, _currentSelected.transform.position);
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void EndTurnReset()
    {
        currentSelected.GetComponent<Planet>().IsSelected = false;
        currentSelected.GetComponent<Planet>().SelectedPointer.SetActive(false);
        _oldSelected.GetComponent<Planet>().IsSelected = false;
        _oldSelected.GetComponent<Planet>().SelectedPointer.SetActive(false);
        _oldSelected = null;
        _currentSelected = null;
    }
}