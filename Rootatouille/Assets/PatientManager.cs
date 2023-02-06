using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public Patient[] Patients;
    private Patient CurrentPatient;

    private void Start()
    {
        SetRandomPatient();
    }

    private void SetRandomPatient()
    {
        int random = Random.Range(0, Patients.Length - 1);
        CurrentPatient = Patients[random];
        CurrentPatient.gameObject.SetActive(true);
        CurrentPatient.Gooooo();
    }

    public void PatientGone(Patient patient)
    {
        SetRandomPatient();
    }
}