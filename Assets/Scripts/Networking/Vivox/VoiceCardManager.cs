using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Vivox;
using UnityEngine;

public class VoiceCardManager : MonoBehaviour
{
    public GameObject rosterItemPrefab;

    Dictionary<string, List<RosterItem>> m_RosterObjects = new Dictionary<string, List<RosterItem>>();

    private void Awake()
    {
        VivoxService.Instance.ParticipantAddedToChannel += OnParticipantAdded;
        VivoxService.Instance.ParticipantRemovedFromChannel += OnParticipantRemoved;
    }

    private void OnDestroy()
    {
        VivoxService.Instance.ParticipantAddedToChannel -= OnParticipantAdded;
        VivoxService.Instance.ParticipantRemovedFromChannel -= OnParticipantRemoved;
    }
    private void OnParticipantAdded(VivoxParticipant participant)
    {
        Debug.Log("participant added");
        GameObject newRosterObject = Instantiate(rosterItemPrefab, this.gameObject.transform);
        RosterItem newRosterItem = newRosterObject.GetComponent<RosterItem>();
        List<RosterItem> thisChannelList;

        if (m_RosterObjects.ContainsKey(participant.ChannelName))
        {
            m_RosterObjects.TryGetValue(participant.ChannelName, out thisChannelList);
            newRosterItem.SetupRosterItem(participant);
            thisChannelList.Add(newRosterItem);
            m_RosterObjects[participant.ChannelName] = thisChannelList;
        }
        else
        {
            thisChannelList = new List<RosterItem>
            {
                newRosterItem
            };
            newRosterItem.SetupRosterItem(participant);
            m_RosterObjects.Add(participant.ChannelName, thisChannelList);
        }
    }
    private void OnParticipantRemoved(VivoxParticipant participant)
    {
        Debug.Log("participant remove");
        if (m_RosterObjects.ContainsKey(participant.ChannelName))
        {
            RosterItem removedItem = m_RosterObjects[participant.ChannelName].FirstOrDefault(p => p.Participant.PlayerId == participant.PlayerId);
            if (removedItem != null)
            {
                m_RosterObjects[participant.ChannelName].Remove(removedItem);
                Destroy(removedItem.gameObject);
            }
            else
            {
                Debug.LogError("Trying to remove a participant that has no roster item.");
            }
        }
    }




}
