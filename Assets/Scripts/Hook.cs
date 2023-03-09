using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Hook : MonoBehaviour
{
    public static Hook instance;
	public float speed;
	public float speedMin;
	public float speedBegin;
	public Vector2 velocity;
	public float maxX;
	public float minX;
	public float minY;
	public float maxY;
	public Transform target;
	Vector3 prePosition;

	public int type;

	public bool isUpSpeed;
	public float timeUpSpeed;

    public GameObject hook, halfHook;
    private Vector3 positionHalfHook, scaleHalfHook;

    public bool cameraOut;

    private void Awake()
    {
		instance = this;
    }

    // Use this for initialization
    void Start () {
		isUpSpeed = false;
		prePosition = transform.localPosition;
	}

	public IEnumerator TimeUpSpeed ()
	{
		while(true){
			if(isUpSpeed)
			{
				timeUpSpeed = timeUpSpeed - 1;
				if(timeUpSpeed <= 0)
					isUpSpeed = false;
			}
			yield return new WaitForSeconds (1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkBackState();
		checkMoveOutCameraView();

		positionHalfHook = halfHook.gameObject.transform.position;
        scaleHalfHook = halfHook.gameObject.transform.localScale;
        if (HookBase.instance.hookState == HookState.Backward && !cameraOut)
        {
            hook.SetActive(false);
            halfHook.SetActive(true);
            if (positionHalfHook.x > 0)
            {
                scaleHalfHook.x = -0.2f;
            }
            else
            {
                scaleHalfHook.x = 0.2f;
            }
            halfHook.transform.localScale = scaleHalfHook;
            cameraOut = false;
        }
        else if (HookBase.instance.hookState == HookState.Ready)
        {
            hook.SetActive(true);
            halfHook.SetActive(false);
        }
    }
	void FixedUpdate() {
		if(HookBase.instance.hookState == HookState.Forward)
		{
			GetComponent<Rigidbody2D>().velocity = velocity * speed;
		}		
		else if(HookBase.instance.hookState == HookState.Backward)
		{
			if (cameraOut)
			{
				GetComponent<Rigidbody2D>().velocity = velocity * speed * 3;
			}
			else
			{
				GetComponent<Rigidbody2D>().velocity = velocity * speed;
			}
		}
	}
	
	bool checkPositionOutBound() {
		return  gameObject.GetComponent<Renderer>().isVisible ;
	}

	public void checkTouchScene() { 	
		if(HookBase.instance.hookState == HookState.Ready)
		{
			HookBase.instance.hookState = HookState.Forward;
			velocity = new Vector2(transform.position.x - target.position.x, 
			                       transform.position.y - target.position.y);
			velocity.Normalize();
			speed = speedBegin;
		}
	}

	void checkMoveOutCameraView() {
		if(HookBase.instance.hookState == HookState.Forward) {
			if(!checkPositionOutBound()) {
                cameraOut = true;
				HookBase.instance.hookState = HookState.Backward;
				velocity = -velocity;
			}
		}
	}

	void checkBackState() {
		if(transform.localPosition.y > maxY && HookBase.instance.hookState == HookState.Backward) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			HookBase.instance.hookState = HookState.Ready;
			transform.localPosition = prePosition;
		}
	}
}
