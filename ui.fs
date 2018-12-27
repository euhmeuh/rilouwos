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

input PASSWORD-INPUT
0 PASSWORD-INPUT s! input-cursor
here 4 c, 4 allot PASSWORD-INPUT s! input-string

\ === State machine ===

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

defer CURRENT-STATE
: state!  ( state-xt -- ) is CURRENT-STATE ;

defer state.dash.locked
defer state.dash.unlock
defer state.dash.unlock.write
defer state.dash
defer state.menu
defer state.call
defer state.call.write
defer state.calling.quiet
defer state.calling.loud
defer state.messages
defer state.discussion
defer state.discussion.write
defer state.contacts
defer state.contact
defer state.contact.edit
defer state.alarms
defer state.alarms.edit

:noname  ( key -- )
  case
    KEY.BACK of what's state.dash.unlock state! endof
  endcase
; is state.dash.locked

: ui.check-password  ( -- state-xt )
  what's state.dash
;

:noname  ( key -- )
  dup KEY.BACK =
  if what's state.dash.locked state!
  else
    dup input.numeric-key?
    if
      PASSWORD-INPUT input.append
      what's state.dash.unlock.write state!
    then
  then
; is state.dash.unlock

:noname  ( key -- )
  dup
  case
    KEY.BACK of
      PASSWORD-INPUT dup input.erase
      input.empty?
      if what's state.dash.unlock state! then
    endof
    KEY.OK of ui.check-password state! endof
  endcase
  input.numeric-key?
  if PASSWORD-INPUT input.append then
; is state.dash.unlock.write

:noname  ( key -- )
  case
    KEY.BACK of what's state.dash.locked state! endof
    KEY.OK of what's state.menu state! endof
  endcase
; is state.dash

: ui.menu-go  ( -- state-xt )
  what's state.dash
;

:noname  ( key -- )
  case
    KEY.BACK of what's state.dash state! endof
    KEY.OK of ui.menu-go state! endof
  endcase
; is state.menu

what's state.dash.locked state!

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
  \ COLOR-PRIMARY -rot
  \ SCREEN-TILE-W 1 draw.fill-tiles
  2drop
  cr ." [-----STATUSBAR-----]"
;

\ === Big Digits ===

: ui.big-digits  ( pox-x pos-y -- )
  2drop
  cr ."      # ### ######    "
  cr ."     ##   #*  #  #    "
  cr ."      # ### ###  #    "
  cr ."      #   #*  #  #    "
  cr ."     ###### ###  #    "
;

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
  draw.cursor! cr
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

: ui.notifications  ( pox-x pos-y -- )
  2drop
  12 0 do cr loop
;

\ === Menu ===

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

here
what's state.dash.locked       , 1  , 0  ,
what's state.dash.unlock       , 4  , 0  ,
what's state.dash.unlock.write , 6  , 1  ,
what's state.dash              , 2  , 3  ,
what's state.menu              , 4  , 5  ,
what's state.call              , 4  , 0  ,
what's state.call.write        , 6  , 7  ,
what's state.calling.quiet     , 4  , 8  ,
what's state.calling.loud      , 4  , 9  ,
what's state.messages          , 4  , 5  ,
what's state.discussion        , 4  , 10 ,
what's state.discussion.write  , 6  , 11 ,
what's state.contacts          , 4  , 5  ,
what's state.contact           , 4  , 12 ,
what's state.contact.edit      , 13 , 14 ,
what's state.alarms            , 4  , 12 ,
what's state.alarms.edit       , 13 , 14 ,
17 3 idxarray MENU-STATES

: ui.menu  ( pos-x pos-y -- )
  COLOR-PRIMARY -rot
  SCREEN-TILE-W 1 draw.fill-tiles

  what's CURRENT-STATE MENU-STATES throw
  dup 2 idx@ swap 1 idx@
  cr
  ." ["
  MENU-LABELS type
  ."  -- "
  MENU-LABELS type
  ." ]"
;

: ui.transition  ( key -- )
  CURRENT-STATE
;

: ui.dash  ( -- )
  2 1 ui.big-digits
  0 6 ui.full-date
  0 7 ui.notifications
;

: ui.unlock ( -- )
  PASSWORD-INPUT s@ input-string count
  cr type
;

: ui.render  ( -- )
  0 0 ui.status-bar
  what's CURRENT-STATE
  case
    what's state.dash.locked of ui.dash endof
    what's state.dash.unlock of ui.dash ui.unlock endof
    what's state.dash.unlock.write of ui.dash ui.unlock endof
    what's state.dash of ui.dash endof
    what's state.menu of
      1 2 draw.cursor! cr s" CALLING" draw.text
      1 3 draw.cursor! cr s" CALL" draw.text
      1 4 draw.cursor! cr s" MESSAGES" draw.text
      1 5 draw.cursor! cr s" CONTACTS" draw.text
      1 6 draw.cursor! cr s" ALARMS" draw.text
      1 7 draw.cursor! cr s" SETTINGS" draw.text
    endof
  endcase
  0 19 ui.menu
;

: ui.show  ( key -- )
  ui.transition
  \ COLOR-SECONDARY screen.fill
  ui.render
  \ screen.blit
;

: ui.loop  ( -- )
  ui.render
  begin
    key dup ui.show
    'q' =
  until
;

