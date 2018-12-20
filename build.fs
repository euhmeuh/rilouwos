\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Build ===
\ Tools for building the final ROM image

$FFFF constant FILE-MAX-SIZE

: >rcopy ( a -- a ) ( R: -- a )
  compile dup
  compile >r
; immediate

: rfetch> ( -- a ) ( R: a -- a )
  compile r>
  compile dup
  compile >r
; immediate

: slurp-file  ( name-str len -- addr len )
  r/o bin open-file throw
  >rcopy file-size throw
  if ." File too large (> 4 GB)" cr abort then
  dup FILE-MAX-SIZE >
  if ." File too large (> " FILE-MAX-SIZE . ." B)" cr abort then

  dup allocate throw
  swap 2dup rfetch> read-file throw
  over <>
  if ." Could not read whole file" cr abort
  then
  r> close-file throw
;

: create-string-word  ( str len "name" -- )
  create
  here 2dup ! over cells cell+ allot
  cell+ swap cmove>
;

: cell-count  ( addr -- addr len )
  dup cell+ swap @
;

: include-raw  ( "file" "name" -- )
  bl lword count 2dup
  >newline ." Include raw file " type cr
  slurp-file
  create-string-word
;

\ usage:
\ include-raw icon.bmp ICON
\ ICON cell-count dump
