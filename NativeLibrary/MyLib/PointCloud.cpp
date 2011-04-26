/*
 *  PointCloud.cpp
 *  MyLib
 *
 *  Created by Matt Sonic on 4/13/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

#include "PointCloud.h"

PointCloud::PointCloud() :
numColliders (0)
{
	for (int i = 0; i < 16; i++) {
		matrix[i] = 0;
	}
}

PointCloud::~PointCloud()
{
}

bool PointCloud::draw (int focalLength, double pixelSize,
					   int width, int height, int* depthMap)
{
	//Find all pixels 
	XnUInt32 nAllIndex = width * height; 
	//Update the depth map 
	const XnDepthPixel* nDepthMap = (const XnDepthPixel*) depthMap;
	//Write all pixel with depth in a projection array 
	XnPoint3D points[nAllIndex];
	int _PixelX = 0;
	int _PixelY = 0;
	for (int _Transfer = 0; _Transfer <= nAllIndex; _Transfer++) 
	{ 
		if (_PixelX == width - 1) //new line if x coordinate is at the right end 
		{ 
			_PixelX = 0; 
			_PixelY ++; 
		} 
		else 
		{ 
			_PixelX ++; 
		} 
		
		//X = (u - 320) * depth_md_[k] * pixel_size_ * 0.001 / F_; 
		//Y = (v - 240) * depth_md_[k] * pixel_size_ * 0.001 / F_; 
		//Z = depth_md_[k] * 0.001; // from mm in meters! 
		points[_Transfer].X = (_PixelX - 320) * nDepthMap[_Transfer] * pixelSize * 0.1 / focalLength; 
		points[_Transfer].Y = (_PixelY - 240) * nDepthMap[_Transfer] * pixelSize * 0.1 / focalLength;
		points[_Transfer].Z = nDepthMap[_Transfer] * 0.1;
	} 
	
	// Draw
	glLoadIdentity();//load identity matrix
	
	glLoadMatrixf (matrix);
	
	// Draw Point Cloud
	glDisable(GL_LIGHTING);
	glDisable(GL_TEXTURE_2D);
	glPointSize(5.0f);
	glBegin(GL_POINTS);
	
	glColor3f(0,0,1.0f);
	for (int i = 0; i < nAllIndex; i++) {
		float x = -points[i].X;
		float y = -points[i].Y;
		float z = points[i].Z;

		// Hit tests
		int pointStride = 10;
		if (i % pointStride == 0) {
			for (int c = 0; c < numColliders; c++) {
				float minX = colliders[c].getMinX();
				float maxX = colliders[c].getMaxX();
				float minY = colliders[c].getMinY();
				float maxY = colliders[c].getMaxY();
				float minZ = colliders[c].getMinZ();
				float maxZ = colliders[c].getMaxZ();
				
				if ((x < maxX && x > minX) && 
					(y < maxY && y > minY) && 
					(z < maxZ && z > minZ)) {
					colliders[c].setHit (true);
				} 
			}
		}
		
		//glColor3f(0,0,1.0f - z*.005f);
		/*
		 glColor3f((sin(z*0.5f) + 1) * 0.5f, 
		 (sin(z*0.05f) + 1) * 0.5f, 
		 (sin(z*0.005f) + 1) * 0.5f);
		 */
		glVertex3f (x, y, z);
	}
	glEnd();
	
	// Draw the calibration cube
	glColor3f (1.0f, 0.0f, 0.0f);
	glBegin (GL_LINE_LOOP);
	glVertex3f (-0.5, -0.5, -0.5); // 1234
	glVertex3f (-0.5,  0.5, -0.5); 
	glVertex3f (0.5,  0.5, -0.5);
	glVertex3f (0.5, -0.5, -0.5);
	glEnd();
	
	glBegin (GL_LINE_LOOP);
	glVertex3f (-0.5, -0.5,  0.5);  //5678
	glVertex3f (-0.5,  0.5,  0.5);
	glVertex3f (0.5,  0.5,  0.5);
	glVertex3f (0.5, -0.5,  0.5);
	glEnd();
	
	glBegin (GL_LINES);
	glVertex3f (-0.5, -0.5, -0.5);  //15
	glVertex3f (-0.5, -0.5,  0.5);
	glVertex3f (-0.5,  0.5, -0.5);  //26
	glVertex3f (-0.5,  0.5,  0.5);
	glVertex3f (0.5,  0.5, -0.5);  //37
	glVertex3f (0.5,  0.5,  0.5);
	glVertex3f (0.5, -0.5, -0.5);  //48
	glVertex3f (0.5, -0.5,  0.5);
	glEnd();
	
	return true;		
}

bool PointCloud::setMatrix (float m0,
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
							float m15)
{
	matrix[0] = m0;
	matrix[1] = m1;
	matrix[2] = m2;
	matrix[3] = m3;
	matrix[4] = m4;
	matrix[5] = m5;
	matrix[6] = m6;
	matrix[7] = m7;
	matrix[8] = m8;
	matrix[9] = m9;
	matrix[10] = m10;
	matrix[11] = m11;
	matrix[12] = m12;
	matrix[13] = m13;
	matrix[14] = m14;
	matrix[15] = m15;
	return true;
}

bool PointCloud::setNumColliders (int numColliders_)
{
	numColliders = numColliders_;
	return true;
}

bool PointCloud::setCollider (int index, 
							  float minX, float maxX,
							  float minY, float maxY,
							  float minZ, float maxZ)
{
	colliders[index].setMinX (minX);
	colliders[index].setMaxX (maxX);
	colliders[index].setMinY (minY);
	colliders[index].setMaxY (maxY);
	colliders[index].setMinZ (minZ);
	colliders[index].setMaxZ (maxZ);
	colliders[index].setHit (false);
	return true;
}

bool PointCloud::isColliderHit (int index)
{
	return colliders[index].isHit();
}



