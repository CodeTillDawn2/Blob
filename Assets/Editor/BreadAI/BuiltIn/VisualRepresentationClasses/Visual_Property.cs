using System;
using System.Collections.Generic;
[Serializable]
public class Visual_Property : Visual_Member
{
    public Visual_Property(string name, string returnType, string memberType, List<Visual_Attribute> attributes) :
        base(name, returnType, memberType, attributes)
    { }


}