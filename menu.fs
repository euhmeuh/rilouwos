\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Menu ===
\ Move in a menu

:struct menu
  ushort menu-cursor
  ushort menu-size
  rptr   menu-items
;struct

: menu.cursor+  { a-menu -- }
  a-menu s@ menu-cursor 1+
  a-menu s@ menu-size mod
  a-menu s! menu-cursor
;

: menu.cursor-  { a-menu -- }
  a-menu s@ menu-cursor
  a-menu s@ menu-size tuck 1- + swap mod
  a-menu s! menu-cursor
;

: menu.show  ( menu -- )
  dup s@ menu-size
  0 do
    cr
    dup s@ menu-cursor i =
    if ." > " then
    i over s@ menu-items array-idx
    1 idx $type
  loop
  drop
;

: menu.go  ( menu -- defer )
  dup s@ menu-cursor swap
  s@ menu-items array-idx
  0 idx@
;

