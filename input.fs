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

variable INPUT-MODE
0 constant INPUT-MODE-NUM
1 constant INPUT-MODE-CLASSIC
2 constant INPUT-MODE-DICT

: input.mode.NUM      INPUT-MODE-NUM INPUT-MODE ! ;
: input.mode.CLASSIC  INPUT-MODE-CLASSIC INPUT-MODE ! ;
: input.mode.DICT     INPUT-MODE-DICT INPUT-MODE ! ;

variable ALPHA-LAST-KEY
variable ALPHA-CURSOR

here
KEY.1REC  , ," 1111"
KEY.2ABC  , ," abc2"
KEY.3DEF  , ," def3"
KEY.4GHI  , ," ghi4"
KEY.5JKL  , ," jkl5"
KEY.6MNO  , ," mno6"
KEY.7PQRS , ," pqrs7"
KEY.8TUV  , ," tuv8"
KEY.9WXYZ , ," wxyz9"
KEY.0SP   , ,"  ,.:?!0"
10 3 addr-array ALPHA-TABLE

: input.alpha.force-new  ALPHA-LAST-KEY off ;

: input.alpha  ( num-key -- char replace? )
  dup ALPHA-TABLE array-find if abort then
  1 idx
  over ALPHA-LAST-KEY @ =
  if ( key addr )
    ALPHA-CURSOR @1+!
    ALPHA-CURSOR @
    over str.size <
    if ALPHA-CURSOR @ +
    else 0 ALPHA-CURSOR ! then
    str.start c@ true
  else
    0 ALPHA-CURSOR !
    str.start c@ false
  then ( key char bool )
  rot ALPHA-LAST-KEY !
;

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

: input.reset  ( input -- )
  0 swap s! input-cursor
;

: input.cursor+  ( input -- )
  dup s@ input-cursor 1+
  swap s! input-cursor
;

: input.cursor-  ( input -- )
  dup s@ input-cursor 1- 0 max
  swap s! input-cursor
;

: (input.can-append?)  ( input -- bool )
  dup s@ input-cursor
  swap s@ input-string c@ <
;

: (input.append-char)  ( char input -- )
  dup dup s@ input-string 1+
  swap s@ input-cursor +
  rot swap c!
  input.cursor+
;

: input.append  { a-key an-input -- }
  a-key input.numeric-key?
  if
    INPUT-MODE @
    case
      INPUT-MODE-NUM of
        an-input (input.can-append?)
        if a-key an-input (input.append-char) then
      endof
      INPUT-MODE-CLASSIC of
        a-key input.alpha ( char replace? )
        if an-input input.cursor-
        else
          an-input (input.can-append?)
          not if drop exit then
        then
        an-input (input.append-char)
      endof
    endcase
  then
;

: input.erase  ( input -- )
  dup input.empty? not
  if input.cursor- else drop then
;

: input.allot  ( len input -- )
  here rot dup c, allot align
  swap s! input-string
;

: input.save  ( dest input -- )
  input.count ( dest orig len )
  rot swap    ( orig dest len )
  2dup swap c!
  swap 1+ swap
  cmove>
;
