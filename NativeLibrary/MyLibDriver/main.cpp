#include "MyLib.h"

#include <iostream>

int main (int argc, char * const argv[]) {
    // insert code here...
    std::cout << "Starting driver...\n";

	int number = HelloWorld();
	std::cout << "Number from Mono: " << number;
	return 0;
}
