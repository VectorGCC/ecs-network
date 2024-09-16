using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class BaseClass2
{
    public static void BSFunc()
    {
        Debug.Log("Derived Static Func, BSF = 22222");
    }
}

public class LuaDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var lua = new LuaEnv();
        lua.DoString(@" local BaseClass2 = CS.BaseClass2
BaseClass2.BSFunc()");
        lua.Dispose();
    }
}
