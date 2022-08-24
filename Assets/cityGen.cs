using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cityGen : MonoBehaviour
{
	public GameObject buildingPrefab;
	public uint amount;
	public Vector3 bound;
	
    void Start()
    {
		var prefabSize = buildingPrefab.transform.GetChild(0).GetComponent<BoxCollider>();
        
        for(uint c = 0; c < amount; c++)
        {
			var newPrefab = GameObject.Instantiate(buildingPrefab);
			
			newPrefab.transform.localScale = new Vector3(
				Random.Range(1, 4),
				Random.Range(1, 8),
				Random.Range(1, 4)
			);
			
			var pos = new Vector3(
				Random.Range(
					bound.x - prefabSize.size.x,
					-bound.x + prefabSize.size.x
				),
				
				newPrefab.transform.localScale.y / 2,
				
				Random.Range(
					bound.z - prefabSize.size.z,
					-bound.z + prefabSize.size.z
				)
			);
			
			Instantiate(newPrefab, pos, Quaternion.identity);
		}
    }
}
