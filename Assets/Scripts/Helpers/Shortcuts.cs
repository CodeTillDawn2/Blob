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


}


