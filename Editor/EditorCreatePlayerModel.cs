﻿using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreatePlayerModel
{

    static private string prefabPath;

    static public string filePath;

    

    [MenuItem("Assets/Create Player Model")]
    static public void BuildAssetBundle()
    {
        bool build = true;
        while(true)
        {
            GameObject obj = Selection.activeGameObject;

            if (obj == null)
            {
                Debug.LogError("PlayerModelDescriptor object not selected");
                build = false;
                break;
            }

            string PlayerModelName = obj.GetComponent<PlayerModelDescriptor>().PlayerModelName;
            string Author = obj.GetComponent<PlayerModelDescriptor>().Author;
            
            
            
            GameObject LeftHand = obj.GetComponent<PlayerModelDescriptor>().lefthand;
            GameObject RightHand = obj.GetComponent<PlayerModelDescriptor>().righthand;
            GameObject head = obj.GetComponent<PlayerModelDescriptor>().head;
            GameObject torso = obj.GetComponent<PlayerModelDescriptor>().torso;
            GameObject model = obj.GetComponent<PlayerModelDescriptor>().model;

            bool customcolors = obj.GetComponent<PlayerModelDescriptor>().CustomColors;
            bool gametextures = obj.GetComponent<PlayerModelDescriptor>().GameModeTextures;
            Material basemat = obj.GetComponent<PlayerModelDescriptor>().baseMat;
            Material gamemat = obj.GetComponent<PlayerModelDescriptor>().gameMat;

            
            

            if (PlayerModelName == null || PlayerModelName == "")
            {
                Debug.LogError("'Player Model Name' field is empty");
                build = false;
                
            }
            
            if (Author == null || Author == "")
            {
                Debug.LogError("'Author' field is empty");
                build = false;
                
            }
            
            if(LeftHand == null)
            {
                Debug.LogError("'LeftHand' Bone is missing");
                build = false;
            }

            if (RightHand == null)
            {
                Debug.LogError("'RightHand' Bone is missing");
                build = false;
            }

            if (head == null)
            {
                Debug.LogError("'head' Bone is missing");
                build = false;
            }

            if (torso == null)
            {
                Debug.LogError("'torso' Bone is missing");
                build = false;
            }

            if (model == null)
            {
                Debug.LogError("'model' gameobject is missing");
                build = false;
            }

            if(customcolors && basemat == null)
            {
                Debug.LogError("'CustomColors' is true but 'Base Material' is missing");
                build = false;
            }
            if (gametextures && gamemat == null)
            {
                Debug.LogError("'GameModeTextures' is true but 'Game Material' is missing");
                build = false;
            }

            break;
        }
        

        

        if (build)
        {
            //Debug.Log("Playermodel building");
            var createplayermodel = new CreatePlayerModel();
            createplayermodel.BuildPlayerModel();
        }
    }



    public void BuildPlayerModel()
    {
        GameObject obj = Selection.activeGameObject;


        string PlayerModelName = obj.GetComponent<PlayerModelDescriptor>().PlayerModelName;
        string Author = obj.GetComponent<PlayerModelDescriptor>().Author;

        bool bool1 = obj.GetComponent<PlayerModelDescriptor>().CustomColors;
        bool bool2 = obj.GetComponent<PlayerModelDescriptor>().GameModeTextures;

        string CustomColors = bool1.ToString();
        string GameModeTextures = bool2.ToString();


        if (!AssetDatabase.IsValidFolder("Assets/Temp"))
        {
            AssetDatabase.CreateFolder("Assets", "Temp");
        }

        

        prefabPath = "Assets/Temp/" + PlayerModelName + ".prefab";
        

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        
        

        var prefabAsset = PrefabUtility.SaveAsPrefabAsset(obj.gameObject, prefabPath);

        GameObject contentsRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        
        contentsRoot.name = "playermodel.ParentObject";

        string newprefabPath = "Assets/Temp/" + contentsRoot.name + ".prefab";
        //Debug.Log("Finding bones to rename");

        var desc = contentsRoot.GetComponent<PlayerModelDescriptor>();

        desc.head.name = "playermodel.head";

        desc.lefthand.name = "playermodel.lefthand";

        desc.righthand.name = "playermodel.righthand";

        desc.torso.name = "playermodel.torso";

        desc.model.name = "playermodel.body";

        desc.model.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;

        //Debug.Log("Renamed bones");

        //Debug.Log(desc);

        Text player_info = contentsRoot.AddComponent<Text>();
        string split = "$";
        string mat = desc.baseMat.name + " (Instance)";
        string gameMat = desc.gameMat.name + " (Instance)";


        List<string> data = new List<string>();

        data.Add(PlayerModelName);
        data.Add(Author);
        data.Add(CustomColors);
        data.Add(GameModeTextures);
        data.Add(mat);
        data.Add(gameMat);

        for (int i = 0; i < data.Count; i++)
        {
            player_info.text += data[i];
            if (i < data.Count - 1)
            {
                player_info.text += split;
            }
        }
        //player_info.text = PlayerModelName + split + Author + split + CustomColors + split + GameModeTextures + split + mat;

        Object.DestroyImmediate(contentsRoot.GetComponent<PlayerModelDescriptor>());
        fingermovement fingerscript = contentsRoot.GetComponent<fingermovement>();
        if (fingerscript != null)
        {
            //Debug.Log("Removed FingerScript");
            Object.DestroyImmediate(contentsRoot.GetComponent<fingermovement>());
        }
        
        PrefabUtility.SaveAsPrefabAsset(contentsRoot, newprefabPath);
        PrefabUtility.UnloadPrefabContents(contentsRoot);

        if (File.Exists(prefabPath))
        {
            File.Delete(prefabPath);
            File.Delete(prefabPath+".meta");
        }

        AssetImporter.GetAtPath(newprefabPath).SetAssetBundleNameAndVariant("playermodel.assetbundle", "");


        string assetBundleDirectory = "Assets/StreamingAssets";

        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        filePath = "Assets/";
        string asset_new = filePath + PlayerModelName;

        string asset_temp = assetBundleDirectory + "/playermodel.assetbundle";

        string gtfile = asset_new + ".gtmodel";
        if (File.Exists(asset_temp))
        {

            File.Delete(asset_temp);
            File.Delete(asset_temp + ".meta");
        }

        //creates assetbundle
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        if (File.Exists(newprefabPath))
        {
            File.Delete(newprefabPath);
            File.Delete(newprefabPath + ".meta");
        }

        /*string asset_manifest = assetBundleDirectory + "/playermodel.assetbundle.manifest";
        Debug.Log(asset_manifest);
        if (File.Exists(asset_manifest))
        {
            File.Delete(asset_manifest);
        }*/

        /*string folder_manifest = assetBundleDirectory + "/PlayerModelOutput";
        //Debug.Log(folder_manifest);
        if (File.Exists(folder_manifest))
        {
            File.Delete(folder_manifest);

            File.Delete(folder_manifest + ".manifest");
        }*/



        if (File.Exists(gtfile))
        {
            File.Delete(gtfile);
            File.Move(asset_temp, gtfile);
            Debug.Log("Updated " + PlayerModelName);

        }
        else
        {
            File.Move(asset_temp, gtfile);
            Debug.Log("Created " + PlayerModelName);
        }


        AssetDatabase.Refresh();
        Debug.ClearDeveloperConsole();
        
    }
}