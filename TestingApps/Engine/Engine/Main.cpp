#include "stdafx.h"
#include "Main.h"
#include <stdio.h>


DLL void DoWork(ProgressCallback progressCallback)
{
	int counter = 0;

	for (; counter <= 100; counter++)
	{
		// do the work...
		if (progressCallback)
		{
			// send progress update
			progressCallback(counter);
		}
	}
}