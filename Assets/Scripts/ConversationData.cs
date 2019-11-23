using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
public class ConversationData : ScriptableObject
{
    public List<string> lines;

}
