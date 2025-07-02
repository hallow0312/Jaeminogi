using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectionSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    UI_Interact _interactUI;

    [Header("SpereSetting")]
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform origin;

    private InteractableObject _currentTarget;

    private void Update()
    {
        _currentTarget = FindClosestInteractable();

        if (_currentTarget != null) Debug.Log(_currentTarget);
    }
    private InteractableObject FindClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(origin.position, interactRadius, interactableLayer);

        float closestDist = Mathf.Infinity;
        InteractableObject closest = null;

        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float dist = Vector3.Distance(origin.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closest = interactable;
                    closestDist = dist;
                }
            }
        }

        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin.position, interactRadius);
    }

}
