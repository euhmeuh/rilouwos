\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === UI ===
\ This file describes the User Interface for RilouwOS
\ It is designed for a 240x320 screen,
\ based on a grid with 16x16 tiles.

240 constant SCREEN-W  \ screen width in pixels
320 constant SCREEN-H  \ screen height in pixels

16 constant TILE-W    \ tile size in pixels
16 constant TILE-H
15 constant SCREEN-TILE-W  \ screen width in tiles
20 constant SCREEN-TILE-H  \ screen height in tiles

 9 constant CHAR-W        \ width of a character
12 constant CHAR-H        \ height of a character
 2 constant CHAR-SEP      \ pixels between each character
21 constant MAX-LINE-LEN  \ maximum characters per screen line

1 constant COLOR-PRIMARY
0 constant COLOR-SECONDARY

\ === Dashboard view ===

\ [-----STATUSBAR-----]
\      # ### ######
\     ##   #*  #  #
\      # ### ###  #
\      #   #*  #  #
\     ###### ###  #
\ Wednesday, Decemb. 10
\ John Doe
\ Hey, whats up? I've b-
\ een wondering if you
\ could give me a hand
\ on building the netwo-
\ rk adapter for Rilouw-
\ OS. Feel free to cont-
\ act me when you got t-
\ he time. Cheers!
\
\
\
\ [-------MENU--------]

\ === Status bar ===

: ui.status-bar  ( pos-x pos-y -- )
  COLOR-PRIMARY -rot
  SCREEN-TILE-W 1 draw.fill-tiles
;

\ === Big Digits ===

: ui.big-digits  ( pox-x pos-y -- ) 2drop ;

\ === Date line ===

\ Months are cut if they pass the 18th character:
\
\ Monday, May 1
\ Sunday, February 5
\ Wednesday, January 30
\ Wednesday, Februa. 30
\ Wednesday, March 30
\ Wednesday, April 30
\ Wednesday, May 30
\ Wednesday, June 30
\ Wednesday, July 30
\ Wednesday, August 30
\ Wednesday, Septem. 30
\ Wednesday, October 30
\ Wednesday, Novemb. 30
\ Wednesday, Decemb. 30
\ Saturday, December 30
\ Saturday, Septemb. 28
18 constant MONTH-CHAR-LIMIT

: ui.full-date  ( pox-x pos-y -- )
  draw.cursor!
  date.now
  3dup date.day-name draw.text
  s" , " draw.text
  swap date.month-name draw.text
  s"  " draw.text
  int->str draw.text
  drop
;

\ === Notifications ===

\ A text line structure: 2px blank | 21 chars (229px) | 2px blank | 5px dash | 2px blank
\ To help reading cut words, a dash is drawn at the end of cut lines
\
\ Notifications span from line 7 to 18 (total: 12 lines)
\ It's easily divisible by 1, 2, 3, 4, then from 5 to 6 it's 2 lines per message
\ Max notifications: 6
\ Notifications from the same contact only display the latest unread message from this contact

: ui.notifications  ( pox-x pos-y -- ) 2drop ;

\ === Menu ===

: ui.menu  ( pox-x pos-y -- )
  COLOR-PRIMARY -rot
  SCREEN-TILE-W 1 draw.fill-tiles
;

: ui.dashboard  ( -- )
  0 0 ui.status-bar
  2 1 ui.big-digits
  0 6 ui.full-date
  0 7 ui.notifications
  0 19 ui.menu
;

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

\ User Interface:
\ Dashboard          [Unlock     ] [Lock   Menu]
\ - Menu             [Back     OK]
\   - Calling        [Back   Loud] [Back  Quiet]
\   - Call           [Back       ] [Erase   Add]
\   - Messages       [Back     OK]
\     - Discussion   [Back  Write] [Erase  Send]
\   - Contacts       [Back     OK]
\     - Contact      [Back   Edit] [Cancel Save]
\   - Alarms         [Back   Edit] [Cancel Save]

variable CURRENT-STATE
: state!  ( state-xt -- ) CURRENT-STATE a! ;

: state.dashboard.locked ;
: state.dashboard.unlock ;
: state.dashboard ;
: state.menu ;

: state.dashboard.locked  ( key -- )
  KEY.BACK = if ' state.dashboard.unlock state! then
;

: ui.check-password  ( -- state-xt )
  ' state.dashboard.unlock
;

: state.dashboard.unlock  ( key -- )
  dup KEY.BACK = if ' state.dashboard.locked state! then
  KEY.OK = if ' ui.check-password state! then
;

: state.dashboard  ( key -- )
  dup KEY.BACK = if ' state.dashboard.locked state! then
  KEY.OK = if ' state.menu state! then
;

: ui.menu-go  ( -- state-xt )
  ' state.dashboard
;

: state.menu  ( key -- )
  dup KEY.BACK = if ' state.dashboard state! then
  KEY.OK = if ' ui.menu-go state! then
;

: state.call ;
: state.call.write ;
: state.calling.quiet ;
: state.calling.loud ;
: state.messages ;
: state.discussion ;
: state.discussion.write ;
: state.contacts ;
: state.contact ;
: state.contact.edit ;
: state.alarms ;
: state.alarms.edit ;

' state.dashboard.locked state!

here
," "
," UNLOCK"
," LOCK"
," MENU"
," BACK"
," OK"
," ERASE"
," ADD"
," LOUD"
," QUIET"
," WRITE"
," SEND"
," EDIT"
," CANCEL"
," SAVE"
15 csarray MENU-LABELS

: ,menu  ( left right state-xt -- )  , , , ;

here
1  0  ' state.dashboard.locked ,menu
4  1  ' state.dashboard.unlock ,menu
2  3  ' state.dashboard        ,menu
4  5  ' state.menu             ,menu
4  0  ' state.call             ,menu
6  7  ' state.call.write       ,menu
4  8  ' state.calling.quiet    ,menu
4  9  ' state.calling.loud     ,menu
4  5  ' state.messages         ,menu
4  10 ' state.discussion       ,menu
6  11 ' state.discussion.write ,menu
4  5  ' state.contacts         ,menu
4  12 ' state.contact          ,menu
13 14 ' state.contact.edit     ,menu
4  12 ' state.alarms           ,menu
13 14 ' state.alarms.edit      ,menu
16 3 idxarray MENU-STATES

: ui.menu  ( state-xt -- )
  MENU-STATES throw
  cell+ dup @ swap cell+ @
  MENU-LABELS type
  ."  -- "
  MENU-LABELS type
;

: ui.transition  ( key -- )
  CURRENT-STATE a@ execute
;

: ui.render-state  ( -- )
  CURRENT-STATE ui.menu
;

: ui.show  ( key -- )
  ui.transition
  \ COLOR-SECONDARY screen.fill
  ui.render-state
  \ screen.blit
;

: ui.loop  ( -- )
  begin
    key dup ui.show
    'q' =
  until
;

