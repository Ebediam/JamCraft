using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftManager : MonoBehaviour
{
    public delegate void ItemSelectDelegate(Icon icon);
    public static ItemSelectDelegate ItemSelectEvent;

    public delegate void UpdateIconsDelegate();
    public static UpdateIconsDelegate UpdateIconsEvent;

    public delegate void ExitCraftDelegate();
    public static ExitCraftDelegate ExitCraftEvent;

    public Icon firstItem;
    public Icon secondItem;
    public Icon craftItem;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public AudioSource craftSFX;
    public AudioSource selectSFX;

    PickupData firstComponent;
    PickupData secondComponent;
    PickupData craftedResult;

    bool second;

    public ReceiptList receiptList;

    // Start is called before the first frame update

    private void Awake()
    {
        ItemSelectEvent += SelectItem;
        ExitCraftEvent += ResetCraftManager;
    }

    public void SelectItem(Icon sourceIcon)
    {
        selectSFX.Play();
        if(!second)
        {
            firstComponent = sourceIcon.pickupData;

            if (firstComponent == secondComponent)
            {
                if (firstComponent.amount <= 1)
                {
                    secondComponent = null;
                    secondItem.ResetIcon();
                }
            }

            firstItem.UpdateCraftElement(firstComponent);
            craftItem.ResetIcon();
            UpdateSelecItemText(firstComponent);
            CheckCombination();
            second = true;
        }else
        {
            

            secondComponent = sourceIcon.pickupData;

            if(firstComponent == secondComponent)
            {
                if(firstComponent.amount <= 1)
                {
                    firstComponent = null;
                    firstItem.ResetIcon();
                }
            }

            secondItem.UpdateCraftElement(secondComponent);
            craftItem.ResetIcon();
            UpdateSelecItemText(secondComponent);
            CheckCombination();
            second = false;
        }

    }

    public void CheckCombination()
    {
        if(!firstComponent || !secondComponent)
        {
            return;
        }


        foreach(CombinationData combinationData in receiptList.combinationList)
        {
            if(firstComponent == combinationData.firstItem)
            {
                if(secondComponent == combinationData.secondItem)
                {
                    CraftItem(combinationData.craftedItem);
                }
            }else if(firstComponent == combinationData.secondItem)
            {
                if(secondComponent == combinationData.firstItem)
                {
                    CraftItem(combinationData.craftedItem);
                }
            }
        }
    }

    public void CraftItem(PickupData craftItemData)
    {
        craftedResult = craftItemData;
        craftItem.UpdateCraftElement(craftItemData);
        craftItem.button.interactable = true;

        nameText.text = craftItemData.name;
        descriptionText.text = craftItemData.description;

    }

    public void GetItem()
    {
        craftSFX.Play();

        if (!firstComponent.remainsAfterCombination)
        {
            firstComponent.amount--;
        }

        if (!secondComponent.remainsAfterCombination)
        {
            secondComponent.amount--;
        }


        GameManager.GiveItemToPlayer(Player.local, craftedResult);
        ResetCraftManager();

    }


    public void UpdateSelecItemText(PickupData item)
    {
        nameText.text = item.name;
        descriptionText.text = item.description;
    }

    public void ResetCraftManager()
    {
        firstComponent = null;
        secondComponent = null;
        craftedResult = null;

        firstItem.ResetIcon();
        secondItem.ResetIcon();
        craftItem.ResetIcon();

        UpdateIconsEvent?.Invoke();
    }
}
