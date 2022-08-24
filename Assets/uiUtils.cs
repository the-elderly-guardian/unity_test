using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class uiUtils
{
    public static VisualElement FindByName(VisualElement parent, string name)
	{
		if (parent.name == name)
		{
			return parent;
		}
		
		for(int i = 0; i < parent.childCount; i++)
		{
			VisualElement result = FindByName(parent[i], name);
			if (result != null) return result;
		}
		return null;
	}
}
