using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

namespace RootMotion {
	public class CameraControllerFPS: MonoBehaviour
	{
		public float rotationSensitivity = 3f;
		public float rotationSensitivityY = 3f;
		public float yMinLimit = -50f;
		public float yMaxLimit = 30f;
		public float xMinLimit = -20f;
		public float xMaxLimit = 20f;
		public float x, y;
		private float Hor=0, Ver=0;
		public float _offsetAngleX; //-15
		public float _offsetAngleY; //10
        void Awake () {
			Vector3 angles = transform.localEulerAngles;
			//x = angles.y+_offsetAngleY;
			//y = angles.x-_offsetAngleX;
		}
		public void M_LateUpdate() {
			//Cursor.lockState = CursorLockMode.Locked;	
			if (Hor != 0 || Ver != 0) 
			{
			   x += Hor* rotationSensitivity;//Input.GetAxis("Horizontal")* rotationSensitivity;
			   y = ClampAngle(y - Ver* rotationSensitivityY, yMinLimit, yMaxLimit);// Input.GetAxis("Vertical")			   															
			   transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);
			   Hor = 0;
			   Ver = 0;
			}
		}

		public void m_LateUpdate()
		{									
			if (Hor != 0 || Ver != 0)
			{
				x = ClampAngle(x + Hor, xMinLimit, xMaxLimit);
				y = ClampAngle((y - Ver), yMinLimit, yMaxLimit);
			}
			transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);
		}

		public void mmLateUpdate()
		{
			float mx=0;
			float my = 0;
			//M_LateUpdate();	
			if (Hor != 0) 
			{
				float speed = 1; //Mathf.Abs(Hor)/ 0.2f;
				if (Hor > 0)
					mx = 0.2f;
				else
					mx = -0.2f;
				  	
				x= ClampAngle(x + mx * speed * rotationSensitivity, xMinLimit, xMaxLimit);
			}
			if (Ver != 0) {
				float speed = 1;// Mathf.Abs(Ver) / 0.2f;
				if (Ver > 0)
					my = 0.2f;
				else
					my = -0.2f;
				y = ClampAngle(y - (my * speed * rotationSensitivityY), yMinLimit, yMaxLimit);
			}
		   transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);					  		  
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
	}
}
