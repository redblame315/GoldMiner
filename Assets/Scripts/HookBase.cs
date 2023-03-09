using UnityEngine;
using System.Collections;

public enum HookState { Ready, Forward, Backward };
public class HookBase : MonoBehaviour {
    public static HookBase instance; 
	public Transform hookTransform;
	public Vector3 angles;
	private float speed = 3;
	public float angleMax = 70;
	public HookState hookState = HookState.Ready;
    public float rotationDay;

	void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        hookTransform = Hook.instance.transform;
    }
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
		gameObject.GetComponent<LineRenderer>().SetPosition(1, hookTransform.position);
	}

	void FixedUpdate() {
		if(speed > 0 && hookState == HookState.Ready)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * speed) * angleMax);
        }
        rotationDay = transform.rotation.z;	
	}
}
