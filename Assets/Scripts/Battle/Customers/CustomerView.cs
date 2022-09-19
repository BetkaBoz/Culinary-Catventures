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
    [SerializeField] List<Image> DebuffIcons;
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Shadow;
    [SerializeField] GameObject debuffs;
    [SerializeField] Hoverable hoverable;
    [SerializeField] List<Hoverable> hoverableDebuffs;
    Customer _customer;
    Animator anim;
    public Customer Customer => _customer;

    string message, header;

    public void SetUp(Customer customer, CustomerSetUp customerSetUp)
    {
        _customer = customer;
        //set customer to given position
        (transform as RectTransform).anchoredPosition = customerSetUp.customerPosition;
        //show customer
        Body.GetComponent<SpriteLibrary>().spriteLibraryAsset = customer.Data.AnimationSprites;
        anim = Body.GetComponent<Animator>();
        //set offsets
        Action.transform.position += new Vector3(0, customer.Data.actionOffset);
        Body.transform.position += new Vector3(0, customer.Data.spriteOffset);
        //rotate customer
        Vector3 target = new Vector3(transform.rotation.x, customerSetUp.customerYRotate, transform.rotation.z);
        Body.transform.Rotate(target);  

        //subscribtion on events from Customer class -> Observer
        customer.OnDamageTaken += TakeDamage; 
        customer.OnDied += Die;
        customer.OnTurnStarted += StartTurn;
        customer.OnTurnEnded += ChangeExpressions;
        customer.OnActionChanged += ActionChange;
        customer.OnDebuffChanged += DebuffChange;

        customer.RandomizeDebuffs();

        StartTurn();
    }

    private void Awake()
    {
        Action.transform.DOScale(1.07f, 0.85f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StartTurn()
    {
        Action.DOFade(1f, 1f);

        hoverable.SetTooltipEnabled(true);

        ChangeExpressions();
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

    private void DebuffChange()
    {
        foreach (var icon in DebuffIcons)
        {
            icon.gameObject.SetActive(false);
        }
        for(int i = 0; i < _customer.CurrentDebuffs.Count; i++)
        {
            DebuffIcons[i].gameObject.SetActive(true);
            DebuffIcons[i].sprite = _customer.Data.DebuffSprites[(int)_customer.CurrentDebuffs[i]];

            switch (_customer.CurrentDebuffs[i])
            {
                case DebuffTypes.Stun:
                    message = "Customer will be unable to perform their action for a turn. Stacking of this effect should increase the number of turns this effect lasts.";
                    header = "STUN";
                    break;
                case DebuffTypes.Captivate:
                    message = "Prevents customer from leaving. Stacking of this effect increases the number of turns this effect lasts.";
                    header = "CAPTIVATE";
                    break;
                case DebuffTypes.Flavourful:
                    message = "Customer's Hunger will get decreased by X at the start of their turn, then X will decrease by 1. Stacking of this effect should increase X.";
                    header = "FLAVORFUL";
                    break;
                case DebuffTypes.WeaknessMeat:
                    message = "Any Meat only food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
                    header = "WEAKNESS: Meat";
                    break;
                case DebuffTypes.WeaknessVegg:
                    message = "Any Vegetarian food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
                    header = "WEAKNESS: Vegetables";
                    break;
                case DebuffTypes.WeaknessMix:
                    message = "Any Mixed food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
                    header = "WEAKNESS: Mix";
                    break;
            }
            hoverableDebuffs[i].SetMessageHeader(message, header);
        }
    }

    private void TakeDamage()
    {
        Body.transform.DOShakeScale(0.5f, 0.1f, 8, 40, true);

        if ((_customer.CurrentAction != 0 || DoesDebuffExist(DebuffTypes.Captivate)) && _customer.Satisfied) Action.DOFade(0f, 1f);

        hoverable.SetTooltipEnabled(false);
    }

    private bool DoesDebuffExist(DebuffTypes find)
    {
        foreach(var debuff in _customer.CurrentDebuffs)
        {
            if (debuff == find)
                return true;
        }
        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    private void ChangeExpressions()
    {
        float turnsLeftRatio = 1f - (float)_customer.TurnsLeft / _customer.Data.Turns;
        // int spriteIndex = (int) Mathf.Lerp(0, (float)_customer.Data.Sprites.Count-1, turnsLeftRatio);
        anim.SetInteger("animStateIndex", (int) Mathf.Lerp(0, (float) _customer.Data.Sprites.Count - 1, turnsLeftRatio));
    }

    private void Die()
    {
        Action.DOFade(0, 2f);
        foreach (var img in DebuffIcons)
        {
            img.DOFade(0, 2f);
        }
        Body.GetComponent<SpriteRenderer>().DOFade(0, 2f).OnComplete(() => { Destroy(gameObject); });
    }

    private void OnDestroy()
    {
        //unsubscribe from events
        _customer.OnDamageTaken -= TakeDamage;  
        _customer.OnDied -= Die;
        _customer.OnTurnStarted -= StartTurn;
        _customer.OnTurnEnded -= ChangeExpressions;
        _customer.OnActionChanged -= ActionChange;
        _customer.OnDebuffChanged -= DebuffChange;
    }
}
