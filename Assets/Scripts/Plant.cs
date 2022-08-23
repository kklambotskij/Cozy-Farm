using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<Sprite> _sprites;

    [SerializeField]
    private GameObject _waterDropParticle;

    [SerializeField]
    private int _progressID;

    private float _growTime = 2;
    private Image _image;
    private int _progressMaxID;
    private bool _readyToHarvest;
    private bool _watered;

    private void Start()
    {
        _image = GetComponent<Image>();
        ResetData();
    }

    private void ResetData()
    {
        _progressMaxID = _sprites.Count - 1;
        _readyToHarvest = false;
        _watered = false;
        ChangeProgress(0);
    }

    public void NextProgress()
    {
        ChangeProgress(_progressID + 1);
    }

    private void ChangeProgress(int newProgressID)
    {
        if (_progressMaxID == 0)
        {
            Debug.LogError(string.Concat("Plant ", name,
                " can't grow because it hasn't sprites"));
            return;
        }
        
        if (newProgressID <= _progressMaxID)
        {
            _progressID = newProgressID;
            _image.sprite = _sprites[_progressID];
            _readyToHarvest = _progressID == _progressMaxID;
        }
        else
        {
            Debug.LogError(string.Concat("Plant ", name,
                " can't grow because it hasn't enough sprites"));
            return;
        }
    }

    public void ToWater()
    {
        Destroy(Instantiate(
           _waterDropParticle,
           transform.position,
           _waterDropParticle.transform.rotation,
           transform), 1);

        if (!_watered)
        {
            _watered = true;
            StartCoroutine(Growing());
        }
    }

    private IEnumerator Growing()
    {
        while (_progressID < _progressMaxID)
        {
            yield return new WaitForSeconds(_growTime);
            NextProgress();
        }
        yield return null;
    }

    public bool IsReadyToHarvest()
    {
        return _readyToHarvest;
    }

    public void OnHarvest()
    {
        if (_readyToHarvest)
        {

        }
    }

    public void OnInteract()
    {
        ToWater();
    }
}