\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Utils ===
\ Simple words and data structures used everywhere

: 1st  ( a b c -- c )  nip nip ;
: 2nd  ( a b c -- b )  drop swap drop ;
: 3rd  ( a b c -- a )  2drop ;

: int->str  ( int -- str size ) s>d <# #s #> ;

: idx  ( addr idx -- addr ) cells + ;
: idx@  ( addr idx -- value )  idx @ ;

: relative  ( addr "name" -- )
  create a, does> a@
;

\ Counted strings array

: (cs-array)  ( index csarray -- str size )
  swap idx a@ count
;

: cs-array  ( here #strings "name" -- )
  create
    0 do
      dup a, dup c@ 1+ + aligned
    loop
    drop
  does> (cs-array)
;

\ Address array

: array-idx  ( index addr-array -- addr )
  2dup @ >= if abort" Index out of bound in addr-array" then
  cell+ swap idx a@
;

: array-find  ( value addr-array -- addr fail? )
  dup @ 0 do
    cell+
    2dup a@ @ =
    if a@ nip false leave then
  loop
  \ if found:  ( addr 0 -- )
  \ otherwise: ( idx addr -- )
  dup if nip true then
;

: addr-array  ( here n size "name" -- )
  create
    -rot dup , \ save n
    0 do
      dup a, over cells + aligned
    loop
    2drop
;
