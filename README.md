# MINI LEB128

Single-file C# utility for reading/writing LEB128 encoded integers.

Because surely I can't be the only person who needs such a thing in C# :)

## What is it?

LEB128 is a compact, variable-length encoding scheme for integers. 7-bit numbers get encoded
as a single byte, and longer numbers use only as many bytes as necessary.

See [the LEB128 Wikipedia entry](https://en.wikipedia.org/wiki/LEB128) for details.

## What is this library?

The single-file library provides extension functions for System.IO.Stream 
that read and write LEB-encoded integers. 

The API consumes and produces integers. When writing, any integer can be passed in (long, short, etc). 
When reading, the results will be returned as long or ulong integers, and calling code 
should decide whether to demote it to int, short, etc. if desired.

### Limitations

Please note that, per LEB128 specifications, calling code needs to pick whether to use
signed or unsigned serialization, as they are represented by different serialized byte patterns.

Also, this library is not capable of reading integers larger than Int64 or UInt64.

## Special thanks

Special thanks go to prior work by http://llvm.org/docs/doxygen/html/LEB128_8h_source.html and
https://github.com/aumcode/nfx/blob/master/Source/NFX/IO/LEB128.cs

## License

This software is released under the BSD License. Feel free to use it for any purpose.
