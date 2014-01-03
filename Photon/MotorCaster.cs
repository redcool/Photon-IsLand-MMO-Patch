// --------------------
// NOTE:
// this declare classes which focus on fx(particle/model/etc effect) things.
// it include fx play cast/active/disactive.
// --------------------
using UnityEngine;
using System.Collections.Generic;

using MasterClass = CharacterMaster;

namespace MotorSpace
{
	// --------------------
	// user class.
	// --------------------
	public class ShareMotorCaster: InterfaceCaster
	{
		// --------------------
		// public method.
		// to do manual construct.
		// --------------------
		protected void malDoConstruct( MasterClass master )
		{
			// base do construct.
			base.malDoConstruct( master );
			//
			animator_ = null;
			matchSpellId_ = new AnimatorParamInt( GlobalTechConfig.ANIMATOR_PARAM_SPELLID );
			matchSpellLife_ = new AnimatorParamInt( GlobalTechConfig.ANIMATOR_PARAM_SPELLLIFE );
			matchActionId_ = new AnimatorParamInt( GlobalTechConfig.ANIMATOR_PARAM_ACTIONID );
			matchDead_ = new AnimatorParamBool( GlobalTechConfig.ANIMATOR_PARAM_DEAD );
			matchSpeed_ = new AnimatorParamSpeed( GlobalTechConfig.ANIMATOR_PARAM_SPEED );
		}
		
		// --------------------
		// user callback.
		// --------------------
		public void morOnStart2()
		{
			GameObject activeActObject = infGetActiveActObject();
			if( activeActObject != null )
			{
				animator_ = activeActObject.GetComponent< Animator >();
			}
		}
		
		// --------------------
		// user callback.
		// --------------------
		public void morOnUpdate()
		{
			if( animator_ == null )
			{
				return; // no update necessary when animator init failed or no any new updated.
			}
			
			// ok, go set all our params in batch.
			if( matchSpellId_ != null )
			{
				if(matchSpellId_.anpIsDirty())
			   		((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrInt",PhotonTargets.Others,"SpellId",matchSpellId_.paramValue_);
				matchSpellId_.anpGoApply( animator_ );

			}
			if( matchSpellLife_ != null )
			{
				if(matchSpellLife_.anpIsDirty())
					((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrInt",PhotonTargets.Others,"SpellLife",matchSpellLife_.paramValue_);
				matchSpellLife_.anpGoApply( animator_ );
				
			}
			if( matchActionId_ != null )
			{
				if(matchActionId_.anpIsDirty())
					((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrInt",PhotonTargets.Others,"ActionId",matchActionId_.paramValue_);
				matchActionId_.anpGoApply( animator_ );

			}
			if( matchDead_ != null )
			{
				if(matchDead_.anpIsDirty())
					((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrBool",PhotonTargets.Others,"Dying",matchDead_.paramValue_);
				matchDead_.anpGoApply( animator_ );
	
			}
			if( matchSpeed_ != null )
			{
				matchSpeed_.anpGoApply( animator_ );	
			}
		
		   //comment it. 	
		   //((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrFloat",PhotonTargets.Others,"Speed",animator_.GetFloat("Speed") );
		   //((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrFloat",PhotonTargets.Others,"Direction",animator_.GetFloat("Direction") );
			float _newspeed = animator_.GetFloat("Speed");		
			if(Mathf.Abs(_newspeed -_lastSpeed) > 0.1){
				((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrFloat",PhotonTargets.Others,"Speed",_newspeed );
				_lastSpeed = _newspeed;
			}
			//
			//if (!isMonster)
			bool _newhurt = animator_.GetBool("Hurt");
			if(_newhurt!=_lastHurt)
			{
		   		((PhotonSpace.AvatarPhotonCaster) infGetMaster().chmGetPhotonHandler()).RPC("setAnimatorAttrBool",PhotonTargets.Others,"Hurt", _newhurt );
				_lastHurt = _newhurt;
			}	
		}
		float _lastSpeed = 0.0f;
		bool _lastHurt = false;
		
		// --------------------
		// user callback.
		// --------------------
		public void morOnLevelLoaded( bool intoFightScene )
		{
			GameObject activeActObject = infGetActiveActObject();
			if( activeActObject != null )
			{
				animator_ = activeActObject.GetComponent< Animator >();
			}
		}
		
		// --------------------
		// public method.
		// get motor animator root position.
		// --------------------
		public Vector3 morGetPosition()
		{
			if( animator_ != null )
			{
				return animator_.transform.position;
			}
			return Vector3.zero;
		}
		
		// --------------------
		// public method.
		// get motor animator face direction.
		// --------------------
		public Vector3 morGetDirection()
		{
			if( animator_ != null )
			{
				return animator_.transform.forward;
			}
			return Vector3.zero;
		}
		
		// --------------------
		// public method.
		// --------------------
		public void morSetMatchSpellIdLife( int iSpellId, int iSpellLife )
		{
			if( matchSpellId_ != null )
				matchSpellId_.anpSetValue( iSpellId );
			if( matchSpellLife_ != null )
				matchSpellLife_.anpSetValue( iSpellLife );
		}
		
		// --------------------
		// public method.
		// --------------------
		public void morSetMatchActionId( int actionId )
		{
			if( matchActionId_ != null )
				matchActionId_.anpSetValue( actionId );
		}
		
		// --------------------
		// public method.
		// to set our animator match speed param.
		// it's also our moving/chasing/running speed.
		// --------------------
		public void morSetMatchSpeed( float iSpeedX, float iSpeedY, float iSpeedZ, bool iLockDirection )
		{
			if( matchSpeed_ != null )
				matchSpeed_.anpSetValue( iSpeedX, iSpeedY, iSpeedZ, iLockDirection );
		}
		
		// --------------------
		// public method.
		// --------------------
		public void morSetMatchSpeed( Vector3 iSpeed, bool iLockDirection )
		{
			if( matchSpeed_ != null )
				matchSpeed_.anpSetValue( iSpeed, iLockDirection );
		}
		
		// --------------------
		// public method.
		// --------------------
		public void morSetMatchDead( bool iDead )
		{
			if( matchDead_ != null )
				matchDead_.anpSetValue( iDead );
		}
		
		// --------------------
		// private properties.
		// --------------------
		private Animator animator_;
		public AnimatorParamInt matchSpellId_;
		public AnimatorParamInt matchSpellLife_;
		public AnimatorParamInt matchActionId_;
		public AnimatorParamBool matchDead_;
		public AnimatorParamSpeed matchSpeed_;
	}
	
	// --------------------
	// user class.
	// motor base class for avatar only.
	// --------------------
	class AvatarMotorCaster: ShareMotorCaster
	{
		// --------------------
		// constructor method.
		// --------------------
		public AvatarMotorCaster( MasterClass ownMaster )
		{
			malDoConstruct( ownMaster );
		}
		
		// --------------------
		// override method.
		// to do our own manual construct.
		// all our construct works should done within this method.
		// --------------------
		protected void malDoConstruct( MasterClass ownMaster )
		{
			base.malDoConstruct( ownMaster );
		}
	}
	
	// --------------------
	// user class.
	// motor base class for monster only.
	// --------------------
	class MonsterMotorCaster: ShareMotorCaster
	{
		// --------------------
		// constructor method.
		// --------------------
		public MonsterMotorCaster( MasterClass ownMaster )
		{
			malDoConstruct( ownMaster );
		}
		
		// --------------------
		// override method.
		// to do our own manual construct.
		// all our construct works should done within this method.
		// --------------------
		protected void malDoConstruct( MasterClass ownMaster )
		{
			base.malDoConstruct( ownMaster );
		}
	}
}