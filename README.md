# CLive
My first try to make an interpreter WIP.

This language uses simplified C-like syntax.

Declare a variable:
```
int a = 1
```

Make binary expressions:
```
int a = 2
int b = 10 * a

a = a - b * (2 - 4)
```

Make comments:
```
// This is a variable!
float a = 0.0

/.
  This is 2 variables!!
  No Way!!
./

float xd = 24.0
a = xd + 24.0
```

You cannot assign a value to a variable of a different type:
```
string word = "wow //"
float number = word // error
```

You can also declare immutable (constant) variables:
```
const int num = 24
num = 100 // error 
```

Data types:
```
int
float
string
bool
null (idk if this counts as a data type)
```

The type system works kinda bad so you cannot make binary expressions of int with float:
```
float a = 1.0
a = 2.0 + 1 // error
// float numbers have to be written with a decimal point and a number after it 
```

Todo:
  - more datatypes
  - if - else and while statements
  - functions
  - simple standard library
  - Classes



Also, the name of the language is pronounced like - `C - Live` not `klive` or something the words are separated.
