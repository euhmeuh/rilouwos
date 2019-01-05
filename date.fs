\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Date ===
\ Manipulate dates and time

here
," Monday"
," Tuesday"
," Wednesday"
," Thursday"
," Friday"
," Saturday"
," Sunday"
7 cs-array DAY-NAMES

here
," January"
," February"
," March"
," April"
," May"
," June"
," July"
," August"
," September"
," October"
," November"
," December"
12 cs-array MONTH-NAMES

: date.now  ( -- year month day ) 2018 12 14 ;

: date.weekday  ( year month day -- day-num )
  swap dup
  3 < if 12 + rot 1- -rot then
  1+ 13 5 */ +
  swap dup 4 /
  swap dup 400 /
  swap dup 100 / -
  + + +
  5 + 7 mod
;

: date.day-name  ( year month day -- str size )
  date.weekday DAY-NAMES ;

: date.month-name  ( month -- str size )
  1- 12 mod MONTH-NAMES
;
