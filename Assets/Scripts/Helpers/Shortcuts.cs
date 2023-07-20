using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class Shortcuts
{

    public static string GetPath(this Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;
        return current.parent.GetPath() + "/" + current.name;
    }

    public static bool Matches(this GameObject obj1, GameObject obj2)
    {
        string path1 = GetPath(obj1.transform);
        string path2 = GetPath(obj2.transform);

        if (path1 == path2) 
        { 
            return true;
        }
        return false;
    }


}


