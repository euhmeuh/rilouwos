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

: csarray  ( here nstrings "name" -- )
  create
    0 do
      dup a, dup c@ 1+ + aligned
    loop
    drop
  does> ( index -- str size )
    swap cells + a@ count
;

: idxarray  ( here n size "name" -- )
  create
    -rot 0 do
      swap dup a, over + aligned
    loop
    2drop
    0 ,
  does>  ( index -- addr )
    swap
    begin
      2dup a@ dup 0 =

    until
;

