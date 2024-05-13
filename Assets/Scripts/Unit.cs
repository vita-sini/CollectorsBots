using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    private float _moveSpeed = 15f;
    private float _pickupRange = 0.5f;
    private float _carryDistance = 0.5f;
    private BaseBot _baseBot;

    private Resource _carriedResource;
    public bool _isBusy = false;

    public void SetDestination(Resource resource)
    {
        if (resource == null || resource.IsBusy) return;

        _carriedResource = resource;
        _carriedResource.SetBusy();
        _isBusy = true;
        StartCoroutine(MoveToResource());
    }

    public void SetBaseBot(BaseBot baseBot)
    {
        _baseBot = baseBot;
    }

    private void PickupResource()
    {
        if (_carriedResource == null) return;

        _carriedResource.transform.SetParent(transform);
        _carriedResource.transform.localPosition = new Vector3(0, 0, 0);
        StartCoroutine(MoveToBaseBot());
    }

    private void DropResource()
    {
        if (_carriedResource == null || _baseBot == null) return;

        _carriedResource.transform.SetParent(null);
        _baseBot.ReturnResource(_carriedResource); 
        _carriedResource.SetFree(); 
        _carriedResource = null; 

        _isBusy = false;
    }

    private IEnumerator MoveToResource()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, _carriedResource.transform.position);
            var moveDirection = (_carriedResource.transform.position - transform.position).normalized;

            if (distance > _pickupRange)
            {
                transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            }
            else
            {
                PickupResource();
                break;
            }

            yield return null;
        }
    }

    private IEnumerator MoveToBaseBot()
    {
        if (_carriedResource == null)
        {
            yield break;
        }

        while (true)
        {
            float distance = Vector3.Distance(transform.position, _baseBot.transform.position);
            var moveDirection = (_baseBot.transform.position - transform.position).normalized;

            if (distance > _carryDistance)
            {
                transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            }
            else
            {
                DropResource();

                if (_carriedResource != null)
                {
                    _baseBot.ReturnResource(_carriedResource);
                }

                break;
            }

            yield return null;
        }
    }
}