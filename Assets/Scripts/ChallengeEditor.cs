using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEditor;

public class ChallengeEditor : MonoBehaviour
{

    public GameObject NewLevelButton;
    public GameObject NewWaveButton;
    public GameObject EnemyButtonsParent;
    public InputField TimeInputField;
    public Text LevelContents;
    public InputField LevelContentInputField;
    public InputField EditWaveInputField;

    public Text[] MVLevelTexts;

    public InputField[] SwapLevelInputFields;

    public GameObject ChallengeEditorParent;
    public GameObject MasterViewParent;

    private ChallengeLevel CurrentLevel;
    private ChallengeWave CurrentWave;
    private ChallengeData ChallengeData;

    private bool MasterViewActive;
    private int MVStartIndex = 0;

    private void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        //lol
        try
        {
            StreamReader reader = new StreamReader("Assets/Resources/ChallengeData.txt", true);
            string challengeText = reader.ReadToEnd();
            ChallengeData = JsonConvert.DeserializeObject<ChallengeData>(challengeText);
        }
        catch
        {
            Debug.Log("no challenge data");
            ChallengeData = new ChallengeData()
            {
                Levels = new List<ChallengeLevel>()
            };
        }
    }

    public void OnToggleMasterViewClicked()
    {
        MasterViewActive = !MasterViewActive;

        MasterViewParent.SetActive(MasterViewActive);
        ChallengeEditorParent.SetActive(!MasterViewActive);

        if (MasterViewActive)
        {
            FillMVLevels();
        }
    }

    private void FillMVLevels()
    {
        for (int i=MVStartIndex; i < MVStartIndex + MVLevelTexts.Length; i++)
        {
            Text mvText = MVLevelTexts[i - MVStartIndex];
            ChallengeLevel level = ChallengeData.Levels[i];
            mvText.text = "LEVEL: " + i;
            for (int j=0; j < level.Waves.Count; j++)
            {
                ChallengeWave wave = level.Waves[j];
                mvText.text += "\n\nWAVE: " + j;
                mvText.text += "\ntime: " + wave.Time;
                for (int k = 0; k < wave.Enemies.Count; k++)
                {
                    mvText.text += "\n" + wave.Enemies[k].EnemyType;
                }
            }
        }
    }

    public void OnMVRightButtonClicked()
    {
        MVStartIndex = Mathf.Clamp(MVStartIndex + 1, 0, ChallengeData.Levels.Count - MVLevelTexts.Length);
        FillMVLevels();
    }

    public void OnMVLeftButtonClicked()
    {
        MVStartIndex = Mathf.Clamp(MVStartIndex - 1, 0, ChallengeData.Levels.Count - MVLevelTexts.Length);
        FillMVLevels();
    }

    public void OnSwapLevelsClicked()
    {
        int index0 = int.Parse(SwapLevelInputFields[0].text);
        int index1 = int.Parse(SwapLevelInputFields[1].text);
        ChallengeLevel temp = ChallengeData.Levels[index0];
        ChallengeData.Levels[index0] = ChallengeData.Levels[index1];
        ChallengeData.Levels[index1] = temp;
        ChallengeData.Levels[index0].Level = index0;
        ChallengeData.Levels[index1].Level = index1;
        FillMVLevels();
    }

    public void OnNewLevelClicked()
    {
        CurrentLevel = new ChallengeLevel()
        {
            Level = ChallengeData.Levels.Count,
            Waves = new List<ChallengeWave>()
        };
    }

    public void OnNewWaveClicked()
    {
        CurrentWave = new ChallengeWave()
        {
            Enemies = new List<ChallengeWaveEnemy>()
        };
    }

    public void OnEnemyClicked(int enemyType)
    {
        CurrentWave.Enemies.Add(new ChallengeWaveEnemy()
                                {
                                    FryType = enemyType < 5 ? FryType.SmallFry : FryType.Boss,
                                    EnemyType = (EnemyType)enemyType
                                });
    }

    public void OnWaveTimeEdited()
    {
        CurrentWave.Time = float.Parse(TimeInputField.text);
    }

    public void OnScrollLevelInfoUpClicked()
    {
        Vector2 currentPos = LevelContents.rectTransform.anchoredPosition;
        Vector2 newPos = currentPos + new Vector2(0.0f, 20.0f);
        LevelContents.rectTransform.anchoredPosition = newPos;
    }

    public void OnScrollLevelInfoDownClicked()
    {
        Vector2 currentPos = LevelContents.rectTransform.anchoredPosition;
        Vector2 newPos = currentPos + new Vector2(0.0f, -20.0f);
        LevelContents.rectTransform.anchoredPosition = newPos;
    }

    public void OnEditWaveInputEdited()
    {
        int level = int.Parse(LevelContentInputField.text);
        int wave = int.Parse(EditWaveInputField.text);
        CurrentWave = ChallengeData.Levels[level].Waves[wave];
        CurrentWave.Enemies.Clear();
        OnLevelContentInputEdited();
    }

    public void OnDeleteWaveClicked()
    {
        int level = int.Parse(LevelContentInputField.text);
        int wave = int.Parse(EditWaveInputField.text);
        ChallengeData.Levels[level].Waves.RemoveAt(wave);
        OnLevelContentInputEdited();
    }

    public void OnLevelContentInputEdited()
    {
        int level = int.Parse(LevelContentInputField.text);
        if (level >= ChallengeData.Levels.Count)
        {
            LevelContents.text = "No Level " + level;
        }
        else
        {
            LevelContents.text = string.Empty;
            ChallengeLevel challengeLevel = ChallengeData.Levels[level];
            for (int i=0; i < challengeLevel.Waves.Count; i++)
            {
                ChallengeWave wave = challengeLevel.Waves[i];
                LevelContents.text += "\n\nWAVE: " + i;
                LevelContents.text += "\ntime: " + wave.Time;
                for (int j=0; j < wave.Enemies.Count; j++)
                {
                    LevelContents.text += "\n" + wave.Enemies[j].EnemyType;
                }
            }

            CurrentLevel = ChallengeData.Levels[level];

        }
    }

    public void OnSaveWaveClicked()
    {
        CurrentLevel.Waves.Add(CurrentWave);
    }

    public void OnSaveLevelClicked()
    {
        ChallengeData.Levels.Add(CurrentLevel);
    }

    public void OnSaveChallengeDataClicked()
    {
#if UNITY_EDITOR
        StreamWriter writer = new StreamWriter("Assets/Resources/ChallengeData.txt", false);
        string challengeText = JsonConvert.SerializeObject(ChallengeData);
        writer.Write(challengeText);
        writer.Close();
        AssetDatabase.ImportAsset("Assets/Resources/ChallengeData.txt");
#endif
    }


}
