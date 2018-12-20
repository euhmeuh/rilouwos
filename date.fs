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
7 csarray DAY-NAMES

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
12 csarray MONTH-NAMES

: date.now  ( -- year month day ) 2018 12 14 ;

: date.weekday  ( year month day -- day-num )
  rot swap -rot ( d m y -- )
  over 3 < if 1- swap 12 + swap then
  100 /mod
  dup 4 / swap 2* -
  swap dup 4 / + +
  swap 1+ 13 5 */ + +
  ( in zeller 0=sat, so -2 for 0=mon )
  2- 7 mod
;

: date.day-name  ( year month day -- str size )
  date.weekday DAY-NAMES ;

: date.month-name  ( month -- str size )
  12 mod MONTH-NAMES
;
