using TMPro;
using UnityEngine;

[NetworkSync]
public struct TyncComponent
{
    public int Value;
}

public class CubeView : EntityView
{
    public TMP_Text Text;

    protected override void ApplyState()
    {
        Debug.Log("12312312");
        //Text.text = Read().Value.ToString();
    }
}