using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New NPC")]
public class NPCData : ScriptableObject
{
    public new string name;

    public ConversationData greetingsConversation;
    public ConversationData questCompletedConversation;
    public ConversationData questNotCompletedConversation;
    public ConversationData exhaustedConversation;

    public List<PickupData> itemRequest;
    public List<int> itemAmount;

    public List<PickupData> rewardItems;
    public List<int> rewardAmount;


    public bool hasGreeted;
    public bool hasQuest;

    public bool questFinished;





}
