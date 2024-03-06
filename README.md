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
float a = 0

/.
  This is 2 variables!!
  No Way!!
./

float xd = 24
a = xd + 24
```

You cannot assign a value to a variable of a different type:
```
string word = "wow //"
float number = word // error
```

Data types:
```
int
float
string
```

Todo:
  - more datatypes
  - if - else and while statements
  - functions
  - simple standard library
  - Classes



Also, the name of the language is pronounced like - `C - Live` not `klive` or something the words are separated.
