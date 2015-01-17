![Lury][Lury]
====

_The Lury Programming Language_

## Overview
The Lury is _new_ programming language for beginners!

The Lury is Python-like, but also includes many idea from D, Ruby, Java and C#. The Lury DOES NOT want to replace existing languages.

This repository is implement for the lury using C#.

- A proposal for the language: https://gist.github.com/nanase/8d17c7baf30f2cacb4bf (on Japanese)
- Language design, specifications: [lury-lang/lury-specification](https://github.com/lury-lang/lury-specification)


## How to Build
### Debian/Ubuntu
1. Install `mono-jay` package.

```sh
$ sudo apt-get install mono-jay
```

2. Open the solution in MonoDevelop 5 or later.
3. Build the solution on __`Debug`__ or __`Release`__ configuration.

### Windows
1. Open the solution in Visual Studio or Xamarin Studio. You don't have to install `jay.exe` (The repository already includes `jay.exe`).

2. Build the solution on __`Debug (Windows)`__ or __`Release (Windows)`__ configuration.

## License
### Lury
[__MIT License__](../master/LICENSE.lury) /
Copyright (C) 2014-2015 Tomona Nanase

### Jay (jay.exe)
__New BSD License__

[Lury]: https://raw.githubusercontent.com/lury-lang/lury/master/logo/lury.png