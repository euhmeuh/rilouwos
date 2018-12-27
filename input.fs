\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Input ===
\ Handle the keyboard and read strings in buffers

\ Keyboard layout:
\ Back         Up            Ok
\ Call2  Left  Enter  Right  Tone
\ Call1        Down          Hang
\ 1 o_o        2 abc         3 def
\ 4 ghi        5 jkl         6 mno
\ 7 pqrs       8 tuv         9 wxyz
\ #            0 _           *

'b' constant KEY.BACK
'o' constant KEY.OK
'i' constant KEY.UP
'k' constant KEY.DOWN
'j' constant KEY.LEFT
'l' constant KEY.RIGHT
'e' constant KEY.ENTER
'c' constant KEY.CALL1
'v' constant KEY.CALL2
'h' constant KEY.HANG
't' constant KEY.TONE
'1' constant KEY.1REC
'2' constant KEY.2ABC
'3' constant KEY.3DEF
'4' constant KEY.4GHI
'5' constant KEY.5JKL
'6' constant KEY.6MNO
'7' constant KEY.7PQRS
'8' constant KEY.8TUV
'9' constant KEY.9WXYZ
'#' constant KEY.HASH
'0' constant KEY.0SP
'*' constant KEY.STAR

: input.numeric-key?  ( key -- bool )
  dup '0' >= swap '9' <= and
;

:struct input
  ubyte input-cursor
  rptr input-string
;struct

: input.count  ( input -- addr len )
  dup s@ input-string 1+ swap
  s@ input-cursor
;

: input.empty?  ( input -- bool ) s@ input-cursor 0= ;

: input.cursor+  ( input -- )
  dup s@ input-cursor 1+
  swap s! input-cursor
;

: input.cursor-  ( input -- )
  dup s@ input-cursor 1-
  swap s! input-cursor
;

: input.append  ( key input -- )
  over input.numeric-key?
  if
    dup s@ input-cursor
    over s@ input-string
    2dup c@ <
    if ( key input cursor string )
      1+ + rot ( input addr key )
      swap c! input.cursor+
    else
      2drop 2drop
    then
  else 2drop then
;

: input.erase  ( input -- )
  dup input.empty? not
  if input.cursor- else drop then
;
