using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BreadAIWindow : OdinMenuEditorWindow
{
    private const string OverviewTabName = "Overview";
    private const string InterfacesTabName = "Interfaces";
    private const string TestTab = "Test";

    Visual_ComponentList visual_ComponentList = new Visual_ComponentList(new List<Visual_Component_Base>());
    Visual_PropertyList visual_PropertyList = new Visual_PropertyList(new List<Visual_Property>());
    Visual_MethodList visual_MethodList = new Visual_MethodList(new List<Visual_Method>());

    private DateTime lastMenuRefresh { get; set; }
    private DateTime lastDomainRefresh { get { return AIEditorBaker.DomainLastRefreshed; } }

    private OdinMenuTree InterfacesTree;
    private string[] tabNames = new string[] { OverviewTabName, InterfacesTabName, TestTab };
    private int selectedTabIndex;

    [MenuItem("BreadAI/Configuration")]
    private static void OpenWindow()
    {
        GetWindow<BreadAIWindow>().Show();
    }




    protected override OdinMenuTree BuildMenuTree()
    {
        Debug.LogWarning("??");
        lastMenuRefresh = DateTime.Now;
        InterfacesTree = new OdinMenuTree();

        for (int i = 0; i < AIBakerData.Instance.BreadInterfaces.Keys.Count; i++)
        {
            
            string key = AIBakerData.Instance.BreadInterfaces.Keys.ToList()[i].ToString();

            Visual_Interface visual_Interface = new Visual_Interface(key, AIBakerData.Instance.BreadInterfaces[key]);
            InterfacesTree.Add(key, visual_Interface);

            visual_ComponentList = new Visual_ComponentList(visual_Interface.Components);
            visual_PropertyList = new Visual_PropertyList(visual_Interface.Properties);
            visual_MethodList = new Visual_MethodList(visual_Interface.Methods);

            OdinMenuItem Components = new OdinMenuItem(InterfacesTree, $"Components ({visual_ComponentList.List.Count})", visual_ComponentList);
            InterfacesTree.MenuItems[i].ChildMenuItems.Add(Components);

            OdinMenuItem Properties = new OdinMenuItem(InterfacesTree, $"Properties ({visual_PropertyList.List.Count})", visual_PropertyList);
            InterfacesTree.MenuItems[i].ChildMenuItems.Add(Properties);

            OdinMenuItem Methods = new OdinMenuItem(InterfacesTree, $"Methods ({visual_MethodList.List.Count})", visual_MethodList);
            InterfacesTree.MenuItems[i].ChildMenuItems.Add(Methods);


        }




        return InterfacesTree;
    }




    protected override void OnGUI()
    {
        bool NewTab = false;
        int NewSelectedTab = GUILayout.Toolbar(selectedTabIndex, tabNames);

        if (selectedTabIndex != NewSelectedTab || lastMenuRefresh < lastDomainRefresh) NewTab = true;
        selectedTabIndex = NewSelectedTab;
        if (NewTab)
        {
            ForceMenuTreeRebuild();
        }

        switch (tabNames[selectedTabIndex])
        {
            case OverviewTabName:
                // Draw your overview content here
                break;
            case InterfacesTabName:
                base.OnGUI(); // Draw the standard menu tree layout
                break;
            case TestTab:
                // Draw your test content here
                break;
        }
    }

}
