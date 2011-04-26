/*
 *  Collider.h
 *  MyLib
 *
 *  Created by Matt Sonic on 4/13/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

#ifndef Collider_H
#define Collider_H

class Collider 
{
public:
	Collider();
	Collider (float minX_, float maxX_,
			  float minY_, float maxY_,
			  float minZ_, float maxZ_);
	~Collider();

	float getMinX();
	void setMinX (float minX_);
	float getMaxX();
	void setMaxX (float maxX_);
	float getMinY();
	void setMinY (float minY_);
	float getMaxY();
	void setMaxY (float maxY_);
	float getMinZ();
	void setMinZ (float minZ_);
	float getMaxZ();
	void setMaxZ (float maxZ_);
	
	bool isHit();
	void setHit (bool hit_);

private:
	float minX;
	float maxX;
	float minY;
	float maxY;
	float minZ;
	float maxZ;
	
	bool hit;
};

#endif
