/*
 *  MyLib.cp
 *  MyLib
 *
 *  Created by Matt Sonic on 3/31/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

#include <iostream>
#include "MyLib.h"
#include "MyLibPriv.h"
#include "PointCloud.h"

static PointCloud pointCloud;

extern "C" {
	bool camTexture (int nTexId , int width, int height, int* dataPtr)
	{
		glBindTexture(GL_TEXTURE_2D, nTexId);
		glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, width, height,
						GL_RGB, GL_UNSIGNED_BYTE, (GLvoid *)dataPtr); 
		return true;
	}

	bool drawGL()
	{
		glLoadIdentity();//load identity matrix
		
		glTranslatef(0.0f,0.0f,-4.0f);//move forward 4 units
		
		glColor3f(0.0f,0.0f,1.0f); //blue color
		
		glBegin(GL_LINE_LOOP);//start drawing a line loop
		glVertex3f(-1.0f,0.0f,0.0f);//left of window
		glVertex3f(0.0f,-1.0f,0.0f);//bottom of window
		glVertex3f(1.0f,0.0f,0.0f);//right of window
		glVertex3f(0.0f,1.0f,0.0f);//top of window
		glEnd();//end drawing of line loop
		return true;
	}

	bool drawPointCloud (int focalLength, double pixelSize,
						 int width, int height, int* depthMap)
	{
		return pointCloud.draw (focalLength, pixelSize, width, height, depthMap);
	}

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
					float m15)
	{
		return pointCloud.setMatrix (m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, 
									 m10, m11, m12, m13, m14, m15);
	}
	
	bool setNumColliders (int numColliders)
	{
		return pointCloud.setNumColliders (numColliders);
	}
	
	bool setCollider (int index,
					  float minX, float maxX,
					  float minY, float maxY,
					  float minZ, float maxZ)
	{
		return pointCloud.setCollider (index, minX, maxX, minY, maxY, minZ, maxZ);
	}
	
	bool isColliderHit (int index)
	{
		return pointCloud.isColliderHit (index);
	}
};



