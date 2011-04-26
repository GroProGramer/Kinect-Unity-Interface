using System;
using System.Runtime.InteropServices;

namespace MyLibMono
{
	public class MyLib
	{
		[DllImport("MyLib")]
		unsafe public static extern bool camTexture (int nTexId, int width, int height, IntPtr dataPtr);
		[DllImport("MyLib")]
		unsafe public static extern bool drawGL ();
		[DllImport("MyLib")]
		unsafe public static extern bool drawPointCloud (int focalLength, double pixelSize, int width, int height, IntPtr depthMap);
		[DllImport("MyLib")]
		unsafe public static extern bool setMatrix (float m0, float m1, float m2, float m3, float m4, float m5, float m6, float m7, float m8, float m9,
		float m10, float m11, float m12, float m13, float m14, float m15);
		[DllImport("MyLib")]
		unsafe public static extern bool setNumColliders (int numColliders);
		[DllImport("MyLib")]
		unsafe public static extern bool setCollider (int index, float minX, float maxX, float minY, float maxY, float minZ, float maxZ);
		[DllImport("MyLib")]
		unsafe public static extern bool isColliderHit (int index);

		unsafe public bool CamTexture (int nTexId, int width, int height, IntPtr dataPtr)
		{
			return camTexture (nTexId, width, height, dataPtr);
		}

		unsafe public bool DrawGL ()
		{
			return drawGL ();
		}

		unsafe public bool DrawPointCloud (int focalLength, double pixelSize, int width, int height, IntPtr depthMap)
		{
			return drawPointCloud (focalLength, pixelSize, width, height, depthMap);
		}

		unsafe public bool SetNumColliders (int numColliders)
		{
			return setNumColliders (numColliders);
		}
			
		unsafe public bool SetCollider (int index, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
		{
			return setCollider (index, minX, maxX, minY, maxY, minZ, maxZ);
		}

		unsafe public bool IsColliderHit (int index)
		{
			return isColliderHit (index);
		}
		
		unsafe public bool SetMatrix (float m0, float m1, float m2, float m3, float m4, float m5, float m6, float m7, float m8, float m9,
		float m10, float m11, float m12, float m13, float m14, float m15)
		{
			return setMatrix (m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15);
		}
	}
}

