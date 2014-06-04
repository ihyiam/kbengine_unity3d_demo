
using UnityEngine;
using KBEngine;
using System.Collections;
using System;
using System.Xml;
using System.Collections.Generic;

public class GameEntity : MonoBehaviour 
{
	public bool isPlayer = false;
	double lastUpdateTime = 0.0;
	
	private Vector3 _position = Vector3.zero;
	private Vector3 _eulerAngles = Vector3.zero;
	private Vector3 _scale = Vector3.zero;
	
	public Vector3 destPosition = Vector3.zero;
	public Vector3 destDirection = Vector3.zero;
	
	private float _speed = 0f;
	
	void Awake ()   
	{
		lastUpdateTime = Time.time;
	}
	
	void Start() 
	{
	}

    public Vector3 position {  
		get
		{
			return _position;
		}

		set
		{
			_position = value;
			
			if(gameObject != null)
				gameObject.transform.position = _position;
		}    
    }  
  
    public Vector3 eulerAngles {  
		get
		{
			return _eulerAngles;
		}

		set
		{
			_eulerAngles = value;
			
			if(gameObject != null)
			{
				gameObject.transform.eulerAngles = _eulerAngles;
			}
		}    
    }  

    public Quaternion rotation {  
		get
		{
			return Quaternion.Euler(_eulerAngles);
		}

		set
		{
			eulerAngles = value.eulerAngles;
		}    
    }  
    
    public Vector3 scale {  
		get
		{
			return _scale;
		}

		set
		{
			_scale = value;
			
			if(gameObject != null)
				gameObject.transform.localScale = _scale;
		}    
    } 

    public float speed {  
		get
		{
			return _speed;
		}

		set
		{
			_speed = value;
		}    
    } 

	
    void FixedUpdate () 
    {
    	if(isPlayer == false)
    		return;
    	
    	KBEngine.Entity player = KBEngineApp.app.player();

    	if(player != null)
    	{
	    	player.position.x = gameObject.transform.position.x;
	    	player.position.y = gameObject.transform.position.y;
	    	player.position.z = gameObject.transform.position.z;
			
	    	player.direction.z = gameObject.transform.rotation.eulerAngles.y;
	    }
    }
    
	void Update () 
	{
		if(isPlayer == true)
			return;
		
		float thisDeltaTime = (float)(Time.time - lastUpdateTime);

		if(Vector3.Distance(eulerAngles, destDirection) > 0.0004f)
		{
			rotation = Quaternion.Slerp(rotation, Quaternion.Euler(destDirection), 8f * thisDeltaTime);
		}

		float dist = Vector3.Distance(new Vector3(destPosition.x, 0f, destPosition.z), 
			new Vector3(position.x, 0f, position.z));

		if(dist > 0.5f)
		{
			float deltaSpeed = (speed * thisDeltaTime);
			
			Vector3 pos = position;

			Vector3 movement = destPosition - pos;
			movement.y = 0f;
			movement.Normalize();
			
			movement *= deltaSpeed;
			
			if(dist > deltaSpeed || movement.magnitude > deltaSpeed)
				pos += movement;
			else
				pos = destPosition;

			pos.y = 1.3f;

			position = pos;
		}
		else
		{
		}

		lastUpdateTime = Time.time;
	}
}

