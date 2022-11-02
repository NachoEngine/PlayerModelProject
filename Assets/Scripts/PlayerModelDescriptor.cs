﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using DitzelGames.FastIK;
using UnityEngine.Events;

public class PlayerModelDescriptor : MonoBehaviour
{
    public string PlayerModelName;
    public string Author;

    public GameObject lefthand;
    public GameObject righthand;
    public GameObject torso;
    public GameObject head;

    public GameObject model;

    public bool CustomColors = false;
    public Material baseMat;

    public bool GameModeTextures = false;
    public Material gameMat;

    static private string prefabPath;

    static public string filePath;

    // Start is called before the first frame update
    void Start()
    {
        lefthand.name = "playermodel.handleft";
        righthand.name = "playermodel.handright";


        GameObject hand_l = GameObject.Find("hand_L");
        GameObject hand_r = GameObject.Find("hand_R");


        GameObject offsetL = new GameObject("offsetL");
        GameObject offsetR = new GameObject("offsetR");


        offsetL.transform.SetParent(hand_l.transform, false);
        offsetR.transform.SetParent(hand_r.transform, false);



        GameObject HandLeft = lefthand;
        GameObject HandRight = righthand;

        GameObject root = torso;

        GameObject poleR = new GameObject("poleR");
        poleR.transform.SetParent(root.transform, false);
        GameObject poleL = new GameObject("poleL");
        poleL.transform.SetParent(root.transform, false);

        poleL.transform.localPosition = new Vector3(-5f, -5f, -10);
        poleR.transform.localPosition = new Vector3(5f, -5f, -10);

        GameObject lefthandpos = new GameObject("playermodel.lefthandpos");
        GameObject righthandpos = new GameObject("playermodel.righthandpos");

        GameObject lefthandparent = HandLeft.transform.parent.gameObject;
        GameObject righthandparent = HandRight.transform.parent.gameObject;

        lefthandpos.transform.SetParent(lefthandparent.transform, false);
        righthandpos.transform.SetParent(righthandparent.transform, false);

        lefthandpos.transform.SetPositionAndRotation(HandLeft.transform.position, HandLeft.transform.rotation);
        righthandpos.transform.SetPositionAndRotation(HandRight.transform.position, HandRight.transform.rotation);


        HandLeft.transform.SetPositionAndRotation(hand_l.transform.position, hand_l.transform.rotation);
        HandRight.transform.SetPositionAndRotation(hand_r.transform.position, hand_r.transform.rotation);

        Quaternion rotL = Quaternion.Euler(HandLeft.transform.localRotation.eulerAngles.x, HandLeft.transform.localRotation.eulerAngles.y, HandLeft.transform.localRotation.eulerAngles.z + 20f);
        Quaternion rotR = Quaternion.Euler(HandRight.transform.localRotation.eulerAngles.x, HandRight.transform.localRotation.eulerAngles.y, HandRight.transform.localRotation.eulerAngles.z - 20f);

        HandLeft.transform.position = hand_l.transform.position;
        HandRight.transform.position = hand_r.transform.position;

        HandLeft.transform.localRotation = rotL;
        HandRight.transform.localRotation = rotR;

        HandLeft.transform.SetParent(hand_l.transform, true);
        HandRight.transform.SetParent(hand_r.transform, true);

        HandLeft = lefthandpos;
        HandRight = righthandpos;

        HandLeft.AddComponent<FastIKFabric>();
        HandLeft.GetComponent<FastIKFabric>().Target = offsetL.transform;
        HandLeft.GetComponent<FastIKFabric>().Pole = poleL.transform;

        HandRight.AddComponent<FastIKFabric>();
        HandRight.GetComponent<FastIKFabric>().Target = offsetR.transform;
        HandRight.GetComponent<FastIKFabric>().Pole = poleR.transform;


        model.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;

        
    }








}

