#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;

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

	[MenuItem("Speck Menu/Clear Score Tutorial Prefs")]
	public static void ClearScoreTutorialPrefs()
	{
		PlayerPrefs.DeleteKey(Utility.PrefsSeenScoreTutorialKey);
	}

	[MenuItem("Speck Menu/Clear Challenge Prefs")]
	public static void ClearChallengePrefs()
	{
		PlayerPrefs.DeleteKey(Utility.PrefsChallengeInfoKey);
	}

	[MenuItem("Speck Menu/Set Challenge Levels Passed")]
	public static void SetChallengeLevelsPassed()
	{
		PlayerPrefs.DeleteKey(Utility.PrefsChallengeInfoKey);
		List<PlayerChallengeLevelInfo> infoList = new List<PlayerChallengeLevelInfo>();
		ChallengeMode mode = ChallengeMode.Hardcore | ChallengeMode.Normal;
		for (int i = 0; i < 10; i++)
		{
			PlayerChallengeLevelInfo info = new PlayerChallengeLevelInfo()
			{
				Level = i,
				Modes = mode
			};
			infoList.Add(info);
		}
		PlayerChallengeInfo playerInfo = new PlayerChallengeInfo();
		playerInfo.ChallengeLevelInfoList = infoList;
		string infoJson = JsonConvert.SerializeObject(playerInfo);
		PlayerPrefs.SetString(Utility.PrefsChallengeInfoKey, infoJson);
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
