using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDescription", menuName = "SkillSystem/SkillDescription", order = 0)]
public class SkillDescriptionSO : ScriptableObject
{
    [ValidateInput(nameof(CompareJson), "Json is not equal with object. Save json.")] [SerializeField]
    private TextAsset _json;

    public SkillDescription Skill;

    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void SaveToJson()
    {
        var path = AssetDatabase.GetAssetPath(this);
        var directory = Path.GetDirectoryName(path);
        var jsonFilePath = Path.Combine(directory, $"{name}.json");
        using (var sw = File.CreateText(jsonFilePath))
        {
            var json = JsonConvert.SerializeObject(Skill, Formatting.Indented);
            sw.Write(json);
        }

        AssetDatabase.Refresh();
        _json = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonFilePath);
        AssetDatabase.Refresh();
    }

    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void LoadFromJson()
    {
        if (!_json)
            return;

        var skill = JsonConvert.DeserializeObject<SkillDescription>(_json.text);
        Skill = skill;
    }

    private bool CompareJson()
    {
        if (_json == null)
            return false;
        var json = JsonConvert.SerializeObject(Skill, Formatting.Indented);
        return json == _json.text;
    }
}