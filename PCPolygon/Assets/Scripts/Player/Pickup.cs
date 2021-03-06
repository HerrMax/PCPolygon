﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Pickup : NetworkBehaviour {

    public float pickupRange;
    public Transform eyes;
    public KeyCode pickup = KeyCode.E;
    public Text pickupText;
    public Inventory inventory;
    public AudioSource aS;
    public AudioClip equipSound;
    public int itemID;

    void Update() {
        if (!isLocalPlayer) { return; }

        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, pickupRange) && hit.transform.tag == "Item") {
            pickupText.text = hit.transform.GetComponent<ItemID>().itemName + " [" + pickup + "]";
            if (Input.GetKeyDown(pickup)) {
                OnPickup(hit.transform.gameObject);
            }
        }
        else
        {
            pickupText.text = null;
        }   
    }

    /*public void OnPickup(GameObject hit)
    {
        itemID = hit.transform.GetComponent<ItemID>().itemID;
        inventory.AddItem(itemID);
        Debug.Log(this.transform.name + " pickup : " + hit.name);
        CmdDestroyItem(hit);
    }

    [Command]
    void CmdDestroyItem(GameObject hit)
    {
        RpcDestroyItem(hit);
    }

    [ClientRpc]
    void RpcDestroyItem(GameObject hit)
    {
        NetworkServer.Destroy(hit);
    }*/

    
    [Client]
    void OnPickup(GameObject hit)
    {
        itemID = hit.transform.GetComponent<ItemID>().itemID;
        if (inventory.Addable(itemID) == true)
        {
            //Debug.Log(this.transform.name + " pickup : " + hit.name);
            CmdPickup(hit);
            NetworkServer.Destroy(hit.transform.gameObject);
            inventory.AddItem(itemID);
        }
    }

    [Command]
    void CmdPickup(GameObject hit)
    {
        NetworkServer.Destroy(hit.transform.gameObject);
    }
}
