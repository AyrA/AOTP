AOTP
====

Implementation of Ayras OTP


Purpose
-------

AOTP allows users to easily encrypt a file or multiple files without the need of a password.

Encrypting will encrypt the first file with a generated key, the second file with the encrypted first file, the third file with the encrypted second file and so on.

Decrypting works in opposite order.

Advantages
----------

- Encrypt multiple files with the same key but use the key only once.
- Files almost indistinguishable from random content.
- Technically requires only one byte of memory for each input and output file, but obviously we use more memory to speed the process up
- works using xor operation, which is present in RISC cpu.
- Allows files to be released in order, one by one. As soon as the second file is released, the first released file can be decrypted and so on, allowing information to be released in a controlled matter.
- All files are of the same size, disallowing people to assume what type it might be from its size (text, audio, video)
- encrypted files contain encrypted headers with the original file name. File name does not needs to be given out and the encrypted file can have any name
- Very fast

Disadvantages
-------------

- Encryption generates a key of the same size as the biggest file (+header)
- Making all file sizes equal wastes storage and bandwidth
- If a file is lost, the file itself and the previously released file cannot be decrypted

Controlled release
------------------

Encrypting files with the provided AOTP Application creates the folowing in the selected destination folder:

**key.bin** - This is the key to decrypt the last file. It should be released last. Releasing the key too early does not allows decrypting of any but the last file.

**enc_X.bin** - Multiple files, starting with X=1. The files should be released in ascending order of X

Key detection
-------------

Somebody dedicated enough to break the encryption could guess the key. This is only possible after the last file has been released.
The Random number generator in this application is cryptographically safe but a bit slower than regular generators. The application contains two other generators for testing purposes.

If the last file is released, somebody could try to generate every possible key until he can decrypt the header successfully. This can be very easily prevented by encrypting an additional file.
just create a text file and name it "DISCARD.txt" or something similar and put some content in it. Release the encrypted text file together with the key.

Header
------

The file header looks like this (the value after the colon indicates the size in bytes):

```<File length:8><Name length:4><Name:<Name length>><0:4><Header length:4>```

| Value        | Bytes | Description                                              |
| ------------ | :---: | -------------------------------------------------------- |
| File Length  |   8   | File content length in bytes (allows discarding padding) |
| Name Length  |   4   | File name length in bytes                                |
| Name         |   ?   | Original file name (no path)                             |
| 0            |   4   | Number 0, indicating end of headers                      |
|Header Length |   4   | Length of header, including this number                  |

File format
-----------

An encrypted file contains the encrypted content, an optional padding and its header.
The padding is not present in the longest file and technically does not needs to be present in other files as well. Adding passing to make all files of the same size reduces the risk of content detection.

AOTP Application
----------------

The AOTP Application only serves as proof of concept. Some Features are missing and are listed below:

- The Application does not supports different headers
- Header system lacks field description
- Cannot encrypt using a pregenerated key
- User cannot decide, if to pad files or not
- Files with invalid chars in header names cannot be decrypted

### Screenshot

![Image](https://raw.githubusercontent.com/AyrA/AOTP/master/screen.png)
