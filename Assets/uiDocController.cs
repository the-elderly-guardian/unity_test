using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class uiDocController : MonoBehaviour
{
	public Camera mainCamera;
	public GameObject dronePrefab;

	public GameObject redTowerSpawn, blueTowerSpawn, pinkTowerSpawn;
	
    void Start()
    {
        var root = this.GetComponent<UIDocument>().rootVisualElement;
        
        DropdownField landfields = (DropdownField)uiUtils.FindByName(root, "landfields");
        
        VisualElement leftBlock = uiUtils.FindByName(root, "leftBlock"),
				rightBlock = uiUtils.FindByName(root, "rightBlock"),
				middleBlock = uiUtils.FindByName(root, "middleBlock");
        
        landfields.RegisterCallback<ChangeEvent<string>>((evt) =>
		{
			GameObject drone;
			
			switch(evt.newValue)
			{
				case "Red tower":
				drone = Instantiate(dronePrefab, new Vector3(
					redTowerSpawn.transform.position.x,
					0.5f,
					redTowerSpawn.transform.position.z
				), Quaternion.identity);
				break;
					
				case "Blue tower":
				drone = Instantiate(dronePrefab, new Vector3(
					blueTowerSpawn.transform.position.x,
					0.5f,
					blueTowerSpawn.transform.position.z
				), Quaternion.identity);
				break;
				
				case "Pink tower":
				drone = Instantiate(dronePrefab, new Vector3(
					pinkTowerSpawn.transform.position.x,
					0.5f,
					pinkTowerSpawn.transform.position.z
				), Quaternion.identity);
				break;
			}
			
			mainCamera.enabled = false;
			GameObject.Find("tpv").GetComponent<Camera>().enabled = true;
			
			landfields.visible = false;
			
			leftBlock.visible = true;
			rightBlock.visible = true;
			middleBlock.visible = true;
		});
    }
}
