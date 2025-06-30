using System;
using UnityEngine;

public class SelectionSystem : MonoBehaviour
{
    [SerializeField] UI_INFO _info;
    [SerializeField] float _interactDistance = 5f;
    private Transform _lastSelection;

    InteractableObject _rayCastingObject;
     
    void Update()
    {

        bool isHit = RayCastingFromMousePosition();
        if (!isHit && _lastSelection != null)
        {
            ResetSelect();
        }
    }
    private void ResetSelect()
    {
        _lastSelection = null;
        _info.ResetText();
    }
    private bool CalculateDistance(RaycastHit hit)
    {
        Vector3 origin = Camera.main.transform.position;
        float distance = Vector3.Distance(origin, hit.point);
        Vector3 direction = (hit.point - origin).normalized;

        if (distance <= _interactDistance)
        {
            Debug.DrawLine(origin, hit.point, Color.green);
            return true;
        }
        else
        {
           
            Debug.DrawLine(origin, origin + direction * _interactDistance, Color.red);
            return false;
        }
    }


    private  void SetInfoInteractName(Transform selectionTransform, InteractableObject interact)
    {
        if (_lastSelection != selectionTransform)
        {
            _lastSelection = selectionTransform;
            Debug.Log("Ray");
            _info.SetInfo(interact.GetInteractName());
        }
    }
    public void TryToInteract()
    {
        Debug.Log("Try");
        if (_rayCastingObject != null)
        {
            Debug.Log(_rayCastingObject);
            _rayCastingObject.Interact();
        }
    }
    private bool RayCastingFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        _rayCastingObject = null;
        if (Physics.Raycast(ray, out hit))
        {
            if (CalculateDistance(hit))
            {
                var selectionTransform = hit.transform;
                InteractableObject interact = selectionTransform.GetComponent<InteractableObject>();

                if (interact)
                {
                    _rayCastingObject = interact;
                    SetInfoInteractName(selectionTransform, interact);
                    return true;
                }
            }
        }
    
        return false;
    }
  
}
