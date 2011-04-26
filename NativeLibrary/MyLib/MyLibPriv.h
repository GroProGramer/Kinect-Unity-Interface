/*
 *  MyLibPriv.h
 *  MyLib
 *
 *  Created by Matt Sonic on 3/31/11.
 *  Copyright 2011 SonicTransfer. All rights reserved.
 *
 */

/* The classes below are not exported */
#pragma GCC visibility push(hidden)

class MyLibPriv
{
	public:
		void HelloWorldPriv(const char *);
};

#pragma GCC visibility pop
