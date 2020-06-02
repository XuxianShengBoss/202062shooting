using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

namespace RootMotion {
	public class CameraControllerFPS1: MonoBehaviour
	{
		public float rotationSensitivity = 3f;
		public float rotationSensitivityY = 3f;
		private float yMinLimit = -50f;
		private float yMaxLimit = 30f;
		public float xMinLimit = -20f;
		public float xMaxLimit = 20f;
		private float x, y;
		private float Hor=0, Ver=0;
		public float _offsetAngleX; //-15
		public float _offsetAngleY; //10
        void Awake () {
			Vector3 angles = transform.localEulerAngles;
			x =NormalizeAngle( angles.y);
			y =NormalizeAngle( angles.x);
			xMinLimit=x -80;
			xMaxLimit=x+80;
		}
		public void LateUpdate() {
			x = ClampAngleX(x + Input.GetAxis("Horizontal")* rotationSensitivity, xMinLimit, xMaxLimit);
			y = ClampAngle(y - Input.GetAxis("Vertical") * rotationSensitivityY, yMinLimit, yMaxLimit);	  
			this.transform.localEulerAngles=new Vector3(y,x,0);
			return; 															
			transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);
			Debug.LogError(x);
		}

		public void m_LateUpdate()
		{
			float mx=0;
			float my = 0;
			//M_LateUpdate();	
			if (Hor != 0) 
			{
				float speed = Mathf.Abs(Hor)/ 0.2f;
				if (Hor > 0)
					mx = 0.2f;
				else
					mx = -0.2f;
				  	
				x= ClampAngle(x + mx * speed * rotationSensitivity, xMinLimit, xMaxLimit);
			}
			if (Ver != 0) {
				float speed = Mathf.Abs(Ver) / 0.2f;
				if (Ver > 0)
					my = 0.2f;
				else
					my = -0.2f;
				y = ClampAngle(y - (my * speed * rotationSensitivityY), yMinLimit, yMaxLimit);
			}
		   transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);					  		  
		}

   private float NormalizeAngle(float angleDegrees)
    {
        while (angleDegrees > 180f)
        {
            angleDegrees -= 360f;
        }

        while (angleDegrees <= -180f)
        {
            angleDegrees += 360f;
        }
        return angleDegrees;
    }
		// Clamping Euler angles
		private float ClampAngle (float angle, float min, float max) {
			if (angle < -360) angle += 360;
			if (angle > 360) angle -= 360;
			return Mathf.Clamp (angle, min, max);
		}
		private float ClampAngleX(float angle, float min, float max)
		{
			if (angle < -360) angle += 360;
			if (angle > 360) angle -= 360;
			return Mathf.Clamp(angle, min, max);
		}

		public void SetHorVer(Vector2 vector2) 
		{
			this.Hor = vector2.x;
			this.Ver = vector2.y;
		}
	}
}
