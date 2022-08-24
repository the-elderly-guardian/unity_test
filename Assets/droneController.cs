using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class droneController : MonoBehaviour
{
	private Rigidbody rbody;
	
	private float upForce = 0f;
	
	private Camera fpv, tpv;
	
	private bool isRotateLeft, isRotateRight, isForward, isBackward;
	private bool isLookLeft, isLookRight;
	
	private Quaternion originalFpvRot;

	float clampAngle(float angle)
	{
		return angle > 180f ? angle - 360f : angle;
	}
	
	void changeFpv()
	{
		fpv.enabled = true;
		tpv.enabled = false;
	}
	
	void changeTpv()
	{
		fpv.enabled = false;
		tpv.enabled = true;
	}
	
	void turnCameraLeft()
	{
		float actualAngle = clampAngle(fpv.transform.localEulerAngles.y);
		if(fpv.enabled && actualAngle >= -90f)
		{
			fpv.transform.Rotate(
				fpv.transform.localRotation.x,
				fpv.transform.localRotation.y - 1f,
				0f
			);
		}
		else if(tpv.enabled)
		{
			tpv.transform.RotateAround(
				rbody.transform.position,
				Vector3.up,
				-2f
			);
		}
	}
	
	void turnCameraRight()
	{
		float actualAngle = clampAngle(fpv.transform.localEulerAngles.y);
		if(fpv.enabled && actualAngle <= 90f)
		{
			fpv.transform.Rotate(
				fpv.transform.localRotation.x,
				fpv.transform.localRotation.y + 1f,
				0f
			);
		}
		else if(tpv.enabled)
		{
			tpv.transform.RotateAround(
				rbody.transform.position,
				Vector3.up,
				2f
			);
		}
	}
	
	void resetFpv()
	{
		if(!fpv.enabled)
		{
			fpv.transform.localRotation = originalFpvRot;
		}
	}
	
	void setup()
	{
		isRotateLeft = isRotateRight = false;
		isLookLeft = isLookRight = false;
		
		rbody = this.GetComponent<Rigidbody>();
		
		fpv = GameObject.Find("fpv").GetComponent<Camera>();
		tpv = GameObject.Find("tpv").GetComponent<Camera>();
		
		originalFpvRot = new Quaternion(
			fpv.transform.rotation.x,
			fpv.transform.rotation.y,
			fpv.transform.rotation.z,
			fpv.transform.rotation.w
		);
	}

    void Start()
    {
		setup();
		
        var root = ((UIDocument)FindObjectOfType(typeof(UIDocument))).rootVisualElement;
        
        Button up = (Button)uiUtils.FindByName(root, "up"),
				down = (Button)uiUtils.FindByName(root, "down"),
				left = (Button)uiUtils.FindByName(root, "left"),
				right = (Button)uiUtils.FindByName(root, "right");
				
		Button forward = (Button)uiUtils.FindByName(root, "forward"),
				backward = (Button)uiUtils.FindByName(root, "backward");
				
		Button firstPersonView = (Button)uiUtils.FindByName(root, "fpv"),
				thirdPersonView = (Button)uiUtils.FindByName(root, "tpv"),
				lookAtLeft = (Button)uiUtils.FindByName(root, "lal"),
				lookAtRight = (Button)uiUtils.FindByName(root, "lar");
			
		up.clickable.activators.Clear();
		up.RegisterCallback<MouseDownEvent>((_) =>
		{
			upForce = rbody.mass * 6f;
		});
		up.RegisterCallback<MouseUpEvent>((_) =>
		{
			upForce = 0f;
		});
		
		down.clickable.activators.Clear();
		down.RegisterCallback<MouseDownEvent>((_) =>
		{
			upForce = -rbody.mass * 6f;
		});
		down.RegisterCallback<MouseUpEvent>((_) =>
		{
			upForce = 0f;
		});
		
		left.clickable.activators.Clear();
		left.RegisterCallback<MouseDownEvent>((_) =>
		{
			isRotateLeft = true;
		});
		left.RegisterCallback<MouseUpEvent>((_) =>
		{
			isRotateLeft = false;
		});
		
		right.clickable.activators.Clear();
		right.RegisterCallback<MouseDownEvent>((_) =>
		{
			isRotateRight = true;
		});
		right.RegisterCallback<MouseUpEvent>((_) =>
		{
			isRotateRight = false;
		});
		
		forward.clickable.activators.Clear();
		forward.RegisterCallback<MouseDownEvent>((_) =>
		{
			isForward = true;
		});
		forward.RegisterCallback<MouseUpEvent>((_) =>
		{
			isForward = false;
		});
		
		backward.clickable.activators.Clear();
		backward.RegisterCallback<MouseDownEvent>((_) =>
		{
			isBackward = true;
		});
		backward.RegisterCallback<MouseUpEvent>((_) =>
		{
			isBackward = false;
		});
		
		firstPersonView.RegisterCallback<ClickEvent>((evt) =>
		{
			changeFpv();
		});
		
		thirdPersonView.RegisterCallback<ClickEvent>((evt) =>
		{
			changeTpv();
			resetFpv();
		});
		
		lookAtLeft.clickable.activators.Clear();
		lookAtLeft.RegisterCallback<MouseDownEvent>((_) =>
		{
			isLookLeft = true;
		});
		lookAtLeft.RegisterCallback<MouseUpEvent>((_) =>
		{
			isLookLeft = false;
		});
		
		lookAtRight.clickable.activators.Clear();
		lookAtRight.RegisterCallback<MouseDownEvent>((_) =>
		{
			isLookRight = true;
		});
		lookAtRight.RegisterCallback<MouseUpEvent>((_) =>
		{
			isLookRight = false;
		});
    }
    
    void FixedUpdate()
    {
		if(isLookLeft)
		{
			turnCameraLeft();
		}
		else if(isLookRight)
		{
			turnCameraRight();
		}
		
		float velocityX = rbody.velocity.x + Physics.gravity.x;
		float velocityY = rbody.velocity.y + Physics.gravity.y;
		float velocityZ = rbody.velocity.z + Physics.gravity.z;
		
		rbody.AddForce(
			-velocityX, 
			-velocityY, 
			-velocityZ, 
			ForceMode.Acceleration
		);
		
		if(isForward)
		{
			if(fpv.enabled)
			{
				rbody.AddForce(fpv.transform.forward * 16f);
			}
			else
			{
				rbody.AddForce(tpv.transform.forward * 16f);
			}
		}
		else if(isBackward)
		{
			if(fpv.enabled)
			{
				rbody.AddForce(-fpv.transform.forward * 16f);
			}
			else
			{
				rbody.AddForce(-tpv.transform.forward * 16f);
			}
		}
		else
		{
			rbody.AddForce(0, upForce, 0);
		}
		
		if(isRotateLeft)
		{
			rbody.transform.Rotate(
				0, 
				rbody.transform.localRotation.y - 1f, 
				0);
		}
		else if(isRotateRight)
		{
			rbody.transform.Rotate(
				0, 
				rbody.transform.localRotation.y + 1f, 
				0);
		}
	}
}
