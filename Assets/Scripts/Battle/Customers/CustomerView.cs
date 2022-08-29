using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;

public class CustomerView : MonoBehaviour, IDropHandler
{
    [SerializeField] Image Action;
    [SerializeField] Image Body;
    [SerializeField] GameObject debuffs;
    [SerializeField] Hoverable hoverable;
    [SerializeField] List<SpriteLibraryAsset> spriteLibraries;
    Customer _customer;
    Animator anim;
    public Customer Customer => _customer;
    public SpriteLibrary spriteLibrary;
    string message, header;

    public void SetUp(Customer customer, CustomerSetUp customerSetUp)
    {
        _customer = customer;
        //set customer to given position
        (transform as RectTransform).anchoredPosition = customerSetUp.customerPosition;
        //show customer
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
        customer.OnActionChanged += ActionChange;

        customer.RandomizeDebuffs();

        StartTurn();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Action.transform.DOScale(1.07f, 0.85f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StartTurn()
    {
        Action.DOFade(1f, 1f);

        hoverable.SetTooltipEnabled(true);
    }

    private void ActionChange()
    {
        Action.sprite = _customer.Data.ActionSprites[_customer.CurrentAction];

        switch (_customer.CurrentAction)
        {
            case 0:
                message = "Customer will leave next round!";
                header = "";
                break;
            case 1:
                message = "If customer won't be fed this round, they will take your Reputation Points";
                header = "Reputation Debuff";
                break;
            case 2:
                message = "If customer won't be fed this round, they will cause Energy Points loss for the next round";
                header = "Energy Debuff";
                break;
        }
        hoverable.SetMessageHeader(message, header);
    }

    private void TakeDamage()
    {
        Body.transform.DOShakeScale(0.5f, 0.1f, 8, 40, true);

        if (_customer.CurrentAction != 0) Action.DOFade(0f, 1f);

        hoverable.SetTooltipEnabled(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    private void ChangeExpressions()
    {
        float t = 1f - (float)_customer.TurnsLeft / _customer.Data.Turns;        //ratio of two variables
        int spriteIndex = (int) Mathf.Lerp(0, (float)_customer.Data.Sprites.Count-1, t);
        //Body.DOFade(1, 0.2f).OnPlay(() => { Body.sprite = _customer.Data.Sprites[spriteIndex]; });
        // _customer.Data.overrider.SetAnimations(_customer.Data.overrideControllers[spriteIndex]);
        anim.SetTrigger("UpdateIdle");
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
        _customer.OnActionChanged -= ActionChange;
    }
}
