using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR

public class MenuItems
{



    [MenuItem("Speck Menu/Copy Sprite Slices")]
    static void CopySpriteSlices()
    {
        SpriteSliceCopy window = (SpriteSliceCopy)EditorWindow.GetWindow(typeof(SpriteSliceCopy));
        window.Show();
    }


    #region PrefsGeneral

    [MenuItem("Speck Menu/Clear All Prefs")]
    public static void ClearAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Speck Menu/Clear Challenge Prefs")]
    public static void ClearChallengePrefs()
    {
        PlayerPrefs.DeleteKey(Utility.PrefsChallengeLevelKey);
        PlayerPrefs.DeleteKey(Utility.PrefsCurrentChallengeLevelKey);
    }

    [MenuItem("Speck Menu/Clear HighScore Prefs")]
    public static void ClearHighScorePrefs()
    {
        PlayerPrefs.DeleteKey(Utility.PrefsHighScoreKey);
    }

    [MenuItem("Speck Menu/Clear Tutorial Prefs")]
    public static void ClearTutorialPrefs()
    {
        PlayerPrefs.DeleteKey(Utility.PrefsPlayedTutorialKey);
    }

    #endregion

    #region SetChallengeLevel

    [MenuItem("Speck Menu/Set Player Challenge Level/0")]
    public static void SetPlayerChallengeLevel0()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 0);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/1")]
    public static void SetPlayerChallengeLevel1()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 1);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/2")]
    public static void SetPlayerChallengeLevel2()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 2);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/3")]
    public static void SetPlayerChallengeLevel3()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 3);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/4")]
    public static void SetPlayerChallengeLevel4()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 4);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/5")]
    public static void SetPlayerChallengeLevel5()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 5);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/6")]
    public static void SetPlayerChallengeLevel6()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 6);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/7")]
    public static void SetPlayerChallengeLevel7()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 7);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/8")]
    public static void SetPlayerChallengeLevel8()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 8);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/9")]
    public static void SetPlayerChallengeLevel9()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 9);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/10")]
    public static void SetPlayerChallengeLevel10()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 10);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/11")]
    public static void SetPlayerChallengeLevel11()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 11);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/12")]
    public static void SetPlayerChallengeLevel12()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 12);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/13")]
    public static void SetPlayerChallengeLevel13()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 13);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/14")]
    public static void SetPlayerChallengeLevel14()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 14);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/15")]
    public static void SetPlayerChallengeLevel15()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 15);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/16")]
    public static void SetPlayerChallengeLevel16()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 16);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/17")]
    public static void SetPlayerChallengeLevel17()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 17);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/18")]
    public static void SetPlayerChallengeLevel18()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 18);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/19")]
    public static void SetPlayerChallengeLevel19()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 19);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/20")]
    public static void SetPlayerChallengeLevel20()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 20);
    }

    [MenuItem("Speck Menu/Set Player Challenge Level/21")]
    public static void SetPlayerChallengeLevel21()
    {
        PlayerPrefs.SetInt(Utility.PrefsChallengeLevelKey, 21);
    }

    #endregion

}

public class SpriteSliceCopy : EditorWindow
{

    Object Source;
    Object Destination;

    bool ExecuteClicked = false;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Copy from: ", EditorStyles.boldLabel);
        Source = EditorGUILayout.ObjectField(Source, typeof(Texture2D), false, GUILayout.Width(300f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Copy to: ", EditorStyles.boldLabel);
        Destination = EditorGUILayout.ObjectField(Destination, typeof(Texture2D), false, GUILayout.Width(300f));
        GUILayout.EndHorizontal();

        GUILayout.Space(30f);
        if (GUILayout.Button("EXECUTE"))
        {
            ExecuteClicked = true;
        }

        if (ExecuteClicked)
        {
            GUILayout.Space(30f);
            if (GUILayout.Button("pls double check and confirm"))
            {
                CopySlices();
                ExecuteClicked = false;
            }
        }
    }

    void CopySlices()
    {
        if (!Source || !Destination)
        {
            Debug.LogError("lolnewb");
            return;
        }

        if (Source.GetType() != typeof(Texture2D) || Destination.GetType() != typeof(Texture2D))
        {
            Debug.LogError("LOLNEWB");
            return;
        }

        string srcPath = AssetDatabase.GetAssetPath(Source);
        TextureImporter srcImporter = AssetImporter.GetAtPath(srcPath) as TextureImporter;
        bool srcImporterIsReadableCache = srcImporter.isReadable;
        srcImporter.isReadable = true;

        string destPath = AssetDatabase.GetAssetPath(Destination);
        TextureImporter destImporter = AssetImporter.GetAtPath(destPath) as TextureImporter;
        bool destImporterIsReadableCache = destImporter.isReadable;
        destImporter.isReadable = true;
        
        destImporter.spriteImportMode = SpriteImportMode.Multiple;
        int metaDataLength = srcImporter.spritesheet.Length;
        SpriteMetaData[] metaDataArray = new SpriteMetaData[metaDataLength];
        Debug.Log("Slice count: " + metaDataLength);

        for (int i = 0; i < metaDataLength; i++)
        {
            SpriteMetaData metaData = srcImporter.spritesheet[i];
            metaDataArray[i] = metaData;
        }
        destImporter.spritesheet = metaDataArray;

        AssetDatabase.ImportAsset(destPath, ImportAssetOptions.ForceUpdate);

        srcImporter.isReadable = srcImporterIsReadableCache;
        destImporter.isReadable = destImporterIsReadableCache;
    }

}

#endif
