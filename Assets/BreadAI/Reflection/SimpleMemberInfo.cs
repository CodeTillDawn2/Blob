using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a simplified version of a member (such as a field or property) in a class or struct, primarily used for reflection optimization purposes.
/// </summary>
[Serializable]
public abstract class SimpleMemberInfo
{
    /// <summary>
    /// Gets or sets the name of the member.
    /// </summary>
    public string MemberName { get; set; }

    /// <summary>
    /// Gets or sets the type of the member (e.g., Field, Property).
    /// </summary>
    public string MemberType { get; set; }

    /// <summary>
    /// Gets or sets the type that declares the member.
    /// </summary>
    public Type declaringTypeName { get; set; }

    /// <summary>
    /// Gets or sets a list of simplified attribute information attached to the member.
    /// </summary>
    public List<SimpleAttributeInfo> Attributes { get; set; } = new List<SimpleAttributeInfo>();

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleMemberInfo"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="type">The type of the member.</param>
    /// <param name="declaringType">The type that declares the member.</param>
    /// <param name="attributeInfos">List of simplified attribute information attached to the member.</param>
    public SimpleMemberInfo(string name, string type, Type declaringType)
    {
        MemberName = name;
        MemberType = type;
        declaringTypeName = declaringType;
        
    }

    /// <summary>
    /// Searches the pre-baked data for a member in the target object that matches the current member's name and type.
    /// </summary>
    /// <param name="target">The target object in which to search for the matching member.</param>
    /// <returns>The matching <see cref="SimpleMemberInfo"/> if found; otherwise, null.</returns>
    public SimpleMemberInfo FindMatchingMember(object target)
    {
        if (AIBakerData.instance.BreadDataMembers.TryGetValue(target.GetType().FullName, out List<SimpleMemberInfo> memberInfos))
        {
            // Search for a matching member in the list
            foreach (var info in memberInfos)
            {
                // Check for matching name and type
                if (info.MemberName == MemberName &&
                   info.MemberType == MemberType)
                {
                    return info;  // Return the matching info
                }
            }
        }

        // If nothing found, return null
        return null;
    }

    public List<SimpleAttributeInfo> ReflectAttributes()
    {
        if (this is SimplePropertyInfo me1)
        {
            return me1.CachedPropertyInfo.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                         .Select(x => new SimpleAttributeInfo((Attribute)x)).ToList();
        }
        else if (this is SimpleMethodInfo me2)
        {
            return me2.CachedMethodInfo.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                         .Select(x => new SimpleAttributeInfo((Attribute)x)).ToList();
        }
        else if (this is SimpleFieldInfo me3)
        {
            return me3.CachedFieldInfo.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                         .Select(x => new SimpleAttributeInfo((Attribute)x)).ToList();
        }


        throw new Exception("Member type not found!");
    }
}
