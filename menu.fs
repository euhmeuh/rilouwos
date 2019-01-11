\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Menu ===
\ Move in a menu

:struct menu
  ushort menu-cursor \ current position
  ushort menu-size   \ max displayed items
  rptr   menu-count  \ addr to #items
  \ following members are execution tokens
  ulong  menu-show  ( index current? -- )
  ulong  menu-go    ( index -- )
;struct

: menu.cursor+  { a-menu -- }
  a-menu s@ menu-cursor 1+
  a-menu s@ menu-count @ mod
  a-menu s! menu-cursor
;

: menu.cursor-  { a-menu -- }
  a-menu s@ menu-cursor
  a-menu s@ menu-count @ tuck 1- + swap mod
  a-menu s! menu-cursor
;

: menu.show  { a-menu -- }
  a-menu s@ menu-size
  a-menu s@ menu-count @
  min
  0 do
    i dup
    a-menu s@ menu-cursor =
    a-menu s@ menu-show execute
  loop
;

: menu.go  ( menu -- )
  dup s@ menu-cursor swap
  s@ menu-go execute
;

