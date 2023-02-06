using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Patient : Interactable
{
    public new InteractableType InteractableType => InteractableType.Patient;
    public UnityEvent OnHeal;
    public UnityEvent OnKill;
    public Transform PatientStandPosition;
    public float PatientMoveTime = 1;
    public float PatientLeaveTime = 2;

    [SerializeField] private SpriteRenderer patientSprite;
    [SerializeField] private bool needsBeetroot;
    [SerializeField] private bool needsMold;
    [SerializeField] private bool needsSunflower;
    [SerializeField, Range(1, 4)] private int diseaseSeverity = 1;

    private bool healed;
    private Vector3 originalPos;

    public void Gooooo() // Made and named minutes from disaster
    {
        originalPos = transform.position;
        healed = false;
        Move();
    }

    public void Move()
    {
        patientSprite.color = new Color(patientSprite.color.r, patientSprite.color.g, patientSprite.color.b, 0);
        patientSprite.DOFade(1, PatientMoveTime);
        transform.DOMove(new Vector3(PatientStandPosition.position.x, PatientStandPosition.position.y, originalPos.z), PatientMoveTime);
    }

    public void Leave()
    {
        patientSprite.DOFade(0, PatientMoveTime);
        transform.DOMove(originalPos, PatientLeaveTime).OnComplete(() => { gameObject.SetActive(false); FindObjectOfType<PatientManager>().PatientGone(this); });
    }

    public bool GiveBrew(Flask flask)
    {
        healed = false;
        Brew brew = flask.ContainedBrew;

        if (!brew.FailedBrew && brew.BrewIngredients.Beetroot == needsBeetroot && brew.BrewIngredients.Mold == needsMold &&
            brew.BrewIngredients.Sunflower == needsSunflower && brew.BrewIngredients.Herbs == diseaseSeverity)
            healed = true;

        if (healed == true && OnHeal != null)
        {
            OnHeal.Invoke();
            Debug.Log("Patient healed!");
        }
        else if (OnKill != null)
        {
            Debug.Log("Patient Killed :(");
            OnKill.Invoke();
        }

        Leave();
        return healed;
    }
}