using TMPro;
using UnityEngine;

public class CubeView : EntityView
{
    public TMP_Text Text;

    protected override void ApplyState()
    {
        Debug.Log("12312312");
        Text.text = Read<SyncComponent>().Value.ToString();
    }
}