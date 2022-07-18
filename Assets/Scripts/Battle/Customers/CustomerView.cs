using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomerView : MonoBehaviour, IDropHandler
{
    [SerializeField] Image Action;
    [SerializeField] Image Body;
    [SerializeField] GameObject debuffs;
    Customer _customer;

    public void SetUp(Customer customer, CustomerSetUp customerSetUp)
    {
        _customer = customer;

        (transform as RectTransform).anchoredPosition = customerSetUp.customerPosition;

        Body.sprite = customer.Data.Sprites[0];     //set Element 0 as default sprite 
        Body.SetNativeSize();
        (transform as RectTransform).sizeDelta = new Vector2(Body.rectTransform.rect.width, Body.rectTransform.rect.height);

        //rotating customer
        Vector3 target = new Vector3(transform.rotation.x, customerSetUp.customerYRotate, transform.rotation.z);
        Body.transform.Rotate(target);

        //subscribtion on events from Customer class -> Observer
        customer.OnDamageTaken += TakeDamage; 
        customer.OnDied += Die;
        customer.OnTurnStarted += StartTurn;
        customer.OnTurnEnded += ChangeExpressions;
    }

    private void StartTurn()
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

    private void TakeDamage()
    {
        Action.DOFade(0f, 1f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    private void ChangeExpressions()
    {
        float t = (float)_customer.TurnsLeft / _customer.Data.Turns;        //ratio of two variables
        int spriteIndex = (int) Mathf.Lerp(0, (float)_customer.Data.Sprites.Count-1, t);
        Body.DOFade(1, 0.2f).OnPlay(() => { Body.sprite = _customer.Data.Sprites[spriteIndex]; });
    }

    private void Die()
    {
        Action.DOFade(0, 2f);
        Body.DOFade(0, 2f).OnComplete(() => { Destroy(gameObject); });
    }

    private void OnDestroy()
    {
        //unsubscribe from events
        _customer.OnDamageTaken -= TakeDamage;  
        _customer.OnDied -= Die;
        _customer.OnTurnStarted -= StartTurn;
        _customer.OnTurnEnded -= ChangeExpressions;
    }
}
