/*
 *  PointCloud.h
 *  MyLib
 *
 *  Created by Matt Sonic on 4/13/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

#ifndef PointCloud_H
#define PointCloud_H

#include <vector>

#include "Collider.h"

#define MAX_COLLIDERS 100

class PointCloud 
{
public:
	PointCloud();
	~PointCloud();

	bool draw (int focalLength, double pixelSize,
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
	
	bool setNumColliders (int numColliders_);
	bool setCollider (int index, 
					  float minX, float maxX,
					  float minY, float maxY,
					  float minZ, float maxZ);
	bool isColliderHit (int index);
	
private:
	float matrix[16];
	
	Collider colliders[MAX_COLLIDERS];
	int numColliders;
};

#endif
