using Photon.MmoDemo.Client;
using UnityEngine;
using System.Collections;
using System.Text;

public class WalkDemo : MonoBehaviour
{
	private Game engine;
	public string _lastAnimation = "Idle";
	public float _lastSendTime;
	// Use this for initialization
	public GameObject LocPlayer;//localPlayer
	public float repeatedRate = 0.1f;
	
	void Start ()
	{
		Debug.Log(" walk demo start");	
		if (LocPlayer!= null)//should check connected or not.
			InvokeRepeating("RepeatSayHi",1, repeatedRate);
		//else not invoke send
	}
	
	public void Initialize(Game engine)
	{
		this.engine = engine;
	}
	
	void OnGUI()
	{
		if (GUILayout.Button("SayHi"))
			//call rpc...
		{	//in some on GUI. post event.
			string rpccmd = string.Format("{0},{1},{2}",0,"SayHi",this.name);
			this.engine.Avatar.SetRpc(Encoding.ASCII.GetBytes(rpccmd));//BitConverter.GetBytes(rpccmd));
			//Local call first. e.g: animator.SayHi()
			//Attack..
			//Event.Dispatch("changeAct",this,"walk");
			return;
			CustomEventObj evt = new CustomEventObj("RPC_SEND");
			evt.arguments.Add("value", rpccmd);
			EventManager.instance.dispatchEvent(evt);
			//
			
		}	
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//
	}	
		//if Actor is controll 
		//this.TrySayHi()
	
	
	//load and save even compare two object to check different.
	//Tod: refactor code. make interface to all networked object.
	public void dumpAnimatorParamters(PhotonStream stream)
	{
		Animator animator = LocPlayer.GetComponent<Animator>();
		if (animator==null)
			return;
		stream.SendNext(animator.GetFloat("Speed"));
		stream.SendNext(animator.GetFloat("Direction"));
		stream.SendNext(animator.GetBool("Attack0101"));
		//stream.SendNext(animator.GetBool("Run"));		
		stream.SendNext(animator.GetBool("Hurt"));
		//stream.SendNext(animator.GetBool("Idle"));
	}
	
	
	public void RepeatSayHi()
	{
		if (GetComponent<Actor>()== null)			
			//this.TrySayHi("fuck",this.engine.Avatar.Id);
			//Todo: add Changed Check.if not no need sent it.			
			if (Time.time > _lastSendTime)
			{				
				PhotonStream stream = new PhotonStream(true,null);
				stream.SendNext("1000");
				stream.SendNext("SayHi");
				dumpAnimatorParamters(stream);
				//Use HashTable
				object[] objs = stream.ToArray();
				Hashtable _params = new Hashtable();
				_params[(byte)0] = objs;
			
			
				SendMessage("TrySayHi", _params);
				_lastSendTime +=0.1f;
			}	
		
	}
	
	/// <summary>
	/// Tries the say hi. test Animation State Synchronized.
	/// </summary>
	/// <param name='histr'>
	/// Histr.
	/// </param>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	public void TrySayHi(Hashtable hiparams )//,string id="0")
	{
		//no engine if not attached to mmoengine?
		this.engine.Avatar.SetRpc(hiparams);
		//		
		object[] contents = hiparams[(byte)0] as object[];
		PhotonStream stream = new PhotonStream(false,contents);		
		string _id = (string)stream.ReceiveNext();
		//Debug.Log(string.Format(" _id = {0} ",_id ));
		return; 
		//In here we should collect animation state .
		//string rpccmd = string.Format("{0},{1},{2}",0,"SayHi",histr);
		//this.engine.Avatar.SetRpc(Encoding.ASCII.GetBytes(rpccmd));//BitConverter.GetBytes(rpccmd));	
	}
	
	
	
	[RPC]
	//invoked by Actor 
	public void SayHi(Hashtable hiparams){
		Debug.Log("rpc be called SayHi. todo: changeAvatarState ");
	  //
		object[] contents = hiparams[(byte)0] as object[];
		PhotonStream stream = new PhotonStream(false,contents);		
		//wrong case ..PhotonStream stream = new PhotonStream(false,hiparams[(byte)0] as Object[]);
		string _id = (string)stream.ReceiveNext();
		string _funcname = (string)stream.ReceiveNext();
		//Todo: optimized packet size. 
		
		float _speed = (float) stream.ReceiveNext();
		float _direction = (float)stream.ReceiveNext();
		bool _attack0101 =  (bool)stream.ReceiveNext();
		//bool _run = (bool)stream.ReceiveNext();
		bool _hurt = (bool)stream.ReceiveNext();
		//bool _idle = (bool)stream.ReceiveNext();		
		//
		Animator animator = GetComponent<Animator>();
		//Animation animation = GetComponent<Animation>();
		//animator.SetBool("Jump",true);
		//animator.SetBool("Run", true);
		
		//animator.SetBool("Idle",_idle);
		animator.SetFloat("Speed",_speed);
		animator.SetFloat("Direction",_direction);
		//animator.SetBool("Run",_run);
		animator.SetBool("Attack0101",_attack0101);
		return;
		
//		if (_lastAnimation == "Run")
//		{
//			//animation.CrossFade("Idle");
//			animator.SetBool("Idle",true);
//			_lastAnimation = "Idle";
//		}	
//		else{ 
//			//animation.CrossFade("Run");  Attack0301ttack0301 
//			//animator.SetBool("Attack0101",true);
//			animator.SetFloat("Speed",0.2f);
//			_lastAnimation = "Run";
//		}
		return;
		//Event.Dispatch("changeAct",this,"walk");
		CustomEventObj evt = new CustomEventObj("AnimationAction_Changed");
		evt.arguments.Add("value", "SayHi");
		EventManager.instance.dispatchEvent(evt);
	} //
}

