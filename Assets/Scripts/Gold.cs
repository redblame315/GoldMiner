using UnityEngine;
using System.Collections;

public class Gold: MonoBehaviour {
    
	public bool isMoveFollow = false;
	public float maxY;
	public int score;
	public float speed;
    
	// Use this for initialization
	void Start () {
		isMoveFollow = false;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

	void FixedUpdate() {
		moveFllowTarget(Hook.instance.transform);
    }

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name == "Hook"
            && HookBase.instance.hookState != HookState.Backward)
        {
			isMoveFollow = true;
            Hook.instance.cameraOut = false;
			HookBase.instance.hookState = HookState.Backward;
			Hook.instance.velocity = -Hook.instance.GetComponent<Hook>().velocity;
			Hook.instance.speed -= this.speed;            
        }        
	}

	void moveFllowTarget(Transform target) {
		if(isMoveFollow) 
		{
            Quaternion tg = Quaternion.Euler(target.parent.transform.rotation.x,
                                             target.parent.transform.rotation.y,
                                              target.parent.transform.rotation.z * 40);
            transform.rotation = Quaternion.Slerp(transform.rotation, tg, 0.5f);
            transform.position = new Vector3(target.position.x,
                                                target.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y / 3,
                                                transform.position.z);

            if (HookBase.instance.hookState == HookState.Ready) {
				Destroy(gameObject);
			}
		}
	}
}
