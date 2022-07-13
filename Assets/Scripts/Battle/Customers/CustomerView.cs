using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomerView : MonoBehaviour, IDropHandler
{
    [SerializeField] Text hunger;
    [SerializeField] Image Action;
    [SerializeField] Image Faces;
    [SerializeField] Image Body;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] GameObject debuffs;
    Customer _customer;

    public void SetUp(Customer customer)
    {
        _customer = customer;

        UpdateTexts();

        //subscribtion on events from Customer class -> Observer
        customer.OnDamageTaken += TakeDemage; 
        customer.OnDied += Die;
        customer.OnTurnStarted += StartTurn;
    }

    public virtual void StartTurn()
    {
        Action.DOFade(1f, 1f);

        //if (!customer.Satisfied)
        //{
        //    switch (ac.CurrentIndex)
        //    {
        //        case 1:
        //            gm.HurtPlayer(5);
        //            break;
        //        case 2:
        //            GameObject temp = Instantiate(debuffs);
        //            gm.BuffPlayer(temp.GetComponent<IBuffable>());
        //            break;
        //    }
        //}
        
    }

    private void TakeDemage()
    {
        UpdateTexts();
        Action.DOFade(0f, 1f);
    }

    private void UpdateTexts()
    {
        hunger.text = $"{_customer.CurrentHunger}";
    }
    public void OnDrop(PointerEventData eventData)
    {
        
    }
    public void Die()
    {
        Action.DOFade(0, 2f);
        Faces.DOFade(0, 2f).OnPlay(() => { Body.DOFade(0, 2f); }).OnComplete(() => 
        {
            Destroy(gameObject); 
        });
    }

    private void OnDestroy()
    {
        //unsubscribe from events
        _customer.OnDamageTaken -= TakeDemage;  
        _customer.OnDied -= Die;
        _customer.OnTurnStarted -= StartTurn;
    }
}
