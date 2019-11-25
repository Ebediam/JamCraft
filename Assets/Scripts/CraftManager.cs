using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    PickupData firstComponent;
    PickupData secondComponent;
    PickupData craftedResult;


    public ReceiptList receiptList;

    // Start is called before the first frame update

    private void Awake()
    {
        ItemSelectEvent += SelectItem;
        ExitCraftEvent += ResetCraftManager;
    }

    public void SelectItem(Icon sourceIcon)
    {
        if (!firstComponent)
        {
            firstComponent = sourceIcon.pickupData;
            firstItem.UpdateCraftElement(firstComponent);
        }else if (!secondComponent)
        {
            secondComponent = sourceIcon.pickupData;
            secondItem.UpdateCraftElement(secondComponent);
            CheckCombination();
        }

    }

    public void CheckCombination()
    {
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
    }

    public void GetItem()
    {
        firstComponent.amount--;
        secondComponent.amount--;
        GameManager.GiveItemToPlayer(Player.local, craftedResult);
        ResetCraftManager();

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
