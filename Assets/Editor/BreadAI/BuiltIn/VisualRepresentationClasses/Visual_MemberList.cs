using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Visual_MemberList<T> where T : Visual_Member
{
    [ShowInInspector]
    [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowFoldout = true, IsReadOnly = false,
    ListElementLabelName = "Name")]
    public List<T> List { get; private set; }

    public Visual_MemberList(List<T> list)
    {
        List = list;
    }
}