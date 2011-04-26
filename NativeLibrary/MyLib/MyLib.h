/*
 *  MyLib.h
 *  MyLib
 *
 *  Created by Matt Sonic on 3/31/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

#ifndef MyLib_
#define MyLib_

/* The classes below are exported */
#pragma GCC visibility push(default)

extern "C" {
	bool camTexture (int nTexId , int width, int height, int* dataPtr);
	bool drawGL();
	bool drawPointCloud (int focalLength, double pixelSize,
						 int width, int height, int* depthMap);	
	bool setMatrix (float m0,
					float m1,
					float m2,
					float m3,
					float m4,
					float m5,
					float m6,
					float m7,
					float m8,
					float m9,
					float m10,
					float m11,
					float m12,
					float m13,
					float m14,
					float m15);
	bool setNumColliders (int numColliders);
	bool setCollider (int index, 
					  float minX, float maxX,
					  float minY, float maxY,
					  float minZ, float maxZ);
	bool isColliderHit (int index);
};

#pragma GCC visibility pop
#endif
