/*
 *  Collider.cpp
 *  MyLib
 *
 *  Created by Matt Sonic on 4/13/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

#include "Collider.h"

Collider::Collider() :
minX (0),
maxX (0),
minY (0),
maxY (0),
minZ (0),
maxZ (0),
hit (false)
{
}

Collider::Collider (float minX_, float maxX_,
					float minY_, float maxY_,
					float minZ_, float maxZ_) :
minX (minX_),
maxX (maxX_),
minY (minY_),
maxY (maxY_),
minZ (minZ_),
maxZ (maxZ_),
hit (false)
{
}

Collider::~Collider()
{
}

float Collider::getMinX()
{
	return minX;
}

void Collider::setMinX (float minX_)
{
	minX = minX_;
}

float Collider::getMaxX()
{
	return maxX;
}

void Collider::setMaxX (float maxX_)
{
	maxX = maxX_;
}

float Collider::getMinY()
{
	return minY;
}

void Collider::setMinY (float minY_)
{
	minY = minY_;
}

float Collider::getMaxY()
{
	return maxY;
}

void Collider::setMaxY (float maxY_)
{
	maxY = maxY_;
}

float Collider::getMinZ()
{
	return minZ;
}

void Collider::setMinZ (float minZ_)
{
	minZ = minZ_;
}

float Collider::getMaxZ()
{
	return maxZ;
}

void Collider::setMaxZ (float maxZ_)
{
	maxZ = maxZ_;
}

bool Collider::isHit()
{
	return hit;
}

void Collider::setHit (bool hit_)
{
	hit = hit_;
}

