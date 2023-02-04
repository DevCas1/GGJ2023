using UnityEngine;
using UnityEngine.Events;

public class Patient : Interactable
{
    public new InteractableType InteractableType => InteractableType.Patient;
    public UnityEvent OnHeal;
    public UnityEvent OnKill;
}