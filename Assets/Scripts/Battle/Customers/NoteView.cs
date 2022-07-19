using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteView : MonoBehaviour
{
    [SerializeField] Text hunger;
    [SerializeField] Image note;
    Customer _customer;

    public void SetUp(Customer customer, CustomerSetUp customerSetUp)
    {
        _customer = customer;

        //set position of notes
        (transform as RectTransform).anchoredPosition = customerSetUp.notePosition;

        //rotating notes
        Vector3 target = new Vector3(transform.rotation.x, customerSetUp.noteYRotate, transform.rotation.z);
        note.transform.Rotate(target);

        UpdateTexts();

        customer.OnDamageTaken += UpdateTexts;
    }

    private void UpdateTexts()
    {
        hunger.text = $"{_customer.CurrentHunger}";
    }

    private void OnDestroy()
    {
        _customer.OnDamageTaken -= UpdateTexts;
    }
}