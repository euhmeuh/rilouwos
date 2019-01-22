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

4 constant PASSWORD-LEN
21 constant NUMBER-LEN

\ === Data that should move to some kind of storage ===

create SETTINGS.PASSWORD ," 1234"

input PASSWORD-INPUT
PASSWORD-LEN PASSWORD-INPUT input.allot

input NUMBER-INPUT
CONTACT-FIELD-LEN NUMBER-INPUT input.allot

input CONTACT-EDIT-INPUT
CONTACT-FIELD-LEN CONTACT-EDIT-INPUT input.allot

here
20 ,
%00000001 ,
," Clara"       4 cells allot
," <3"          5 cells allot
," 0123456789"  3 cells allot
%00000001 ,
," Pierre"        4 cells allot
," Bro"           5 cells allot
," 0246801357"    3 cells allot
%00000001 ,
," Simon"       4 cells allot
," Best friend" 3 cells allot
," 0864297531"  3 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000001" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000002" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000003" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000004" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000005" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000006" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000007" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000008" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000009" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000010" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000011" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000012" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000013" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000014" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000015" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000016" 2 cells allot
%00000000 ,
," John Doe"     3 cells allot
," Some guy"     3 cells allot
," +00000000017" 2 cells allot
236 CONTACT-LEN * allot \ free space
relative CONTACT-LIST

variable CURRENT-CONTACT

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

menu MAIN-MENU
menu CONTACTS-MENU
menu CONTACT-MENU

0 constant CONTACT-MENU-CALL
1 constant CONTACT-MENU-MESSAGES
2 constant CONTACT-MENU-NAME
3 constant CONTACT-MENU-DESC
4 constant CONTACT-MENU-NUMBER

: ui.contact-menu.field?  ( idx -- addr|false )
  false swap
  case
    CONTACT-MENU-NAME of
      CURRENT-CONTACT @ contact.name endof
    CONTACT-MENU-DESC of
      CURRENT-CONTACT @ contact.desc endof
    CONTACT-MENU-NUMBER of
      CURRENT-CONTACT @ contact.number endof
  endcase
  dup if nip then
;

defer CURRENT-STATE
: state!  ( state-xt -- ) is CURRENT-STATE ;

defer state.dash.locked
defer state.dash.unlock
defer state.dash.unlock.write
defer state.dash
defer state.menu
defer state.call
defer state.call.write
defer state.calling
defer state.calling.loud
defer state.messages
defer state.discussion
defer state.discussion.write
defer state.contacts
defer state.contact
defer state.contact.edit
defer state.alarms
defer state.alarms.edit

\ === Special transitions ===

: ui.check-password  ( -- state-xt )
  SETTINGS.PASSWORD count
  PASSWORD-INPUT input.count
  compare 0=
  if what's state.dash
  else what's state.dash.locked
  then
  PASSWORD-INPUT input.reset
;

: ui.add-contact  ( -- )
  CONTACT-LIST contacts.new
  dup contact.number NUMBER-INPUT input.save
  CURRENT-CONTACT !
  what's state.contact state!
;

: ui.save-contact  ( -- )
  CONTACT-MENU s@ menu-cursor
  ui.contact-menu.field? ?dup not if abort then
  CONTACT-EDIT-INPUT input.save
  CONTACT-EDIT-INPUT input.reset
  what's state.contact state!
;

\ === States implementation ===

\ Small macro to ease definition of defer
\ Instead of      :noname ... ; is name
\ you can write   :defer name ... ;
: :defer align here code> postpone is ] ;

:defer state.dash.locked  ( key -- )
  case
    KEY.BACK of what's state.dash.unlock state! endof
  endcase
;

:defer state.dash.unlock  ( key -- )
  case
    KEY.BACK of what's state.dash.locked state! endof
    dup input.numeric?
    if
      dup PASSWORD-INPUT INPUT-NUM input.append
      what's state.dash.unlock.write state!
    then
  endcase
;

:defer state.dash.unlock.write  ( key -- )
  case
    KEY.BACK of
      PASSWORD-INPUT dup input.erase
      input.empty?
      if what's state.dash.unlock state! then
    endof
    KEY.OK of ui.check-password state! endof
    dup PASSWORD-INPUT INPUT-NUM input.append
  endcase
;

:defer state.dash  ( key -- )
  case
    KEY.BACK of what's state.dash.locked state! endof
    KEY.OK of what's state.menu state! endof
  endcase
;

:defer state.menu  ( key -- )
  case
    KEY.BACK of what's state.dash state! endof
    KEY.OK of MAIN-MENU menu.go endof
    KEY.UP of MAIN-MENU menu.cursor- endof
    KEY.DOWN of MAIN-MENU menu.cursor+ endof
  endcase
;

:defer state.call  ( key -- )
  case
    KEY.BACK of what's state.menu state! endof
    dup input.numeric?
    if
      dup NUMBER-INPUT INPUT-NUM input.append
      what's state.call.write state!
    then
  endcase
;

:defer state.call.write  ( key -- )
  case
    KEY.BACK of
      NUMBER-INPUT dup input.erase
      input.empty?
      if what's state.call state! then
    endof
    KEY.OK of ui.add-contact endof
    KEY.CALL1 of what's state.calling state! endof
    dup NUMBER-INPUT INPUT-NUM input.append
  endcase
;

:defer state.calling  ( key -- )
  case
    KEY.BACK of what's state.menu state! endof
  endcase
;

:defer state.messages  ( key -- )
  case
    KEY.BACK of what's state.menu state! endof
  endcase
;

:defer state.discussion  ( key -- )
  case
    KEY.BACK of what's state.messages state! endof
  endcase
;

:defer state.contacts  ( key -- )
  case
    KEY.BACK of what's state.menu state! endof
    KEY.OK of CONTACTS-MENU menu.go endof
    KEY.UP of CONTACTS-MENU menu.cursor- endof
    KEY.DOWN of CONTACTS-MENU menu.cursor+ endof
  endcase
;

:defer state.contact  ( key -- )
  case
    KEY.BACK of what's state.contacts state! endof
    KEY.OK of CONTACT-MENU menu.go endof
    KEY.UP of CONTACT-MENU menu.cursor- endof
    KEY.DOWN of CONTACT-MENU menu.cursor+ endof
  endcase
;

:defer state.contact.edit  ( key -- )
  case
    KEY.BACK of what's state.contact state! endof
    KEY.OK of ui.save-contact endof
    KEY.1REC of input.alpha.toggle-caps endof
    dup CONTACT-EDIT-INPUT INPUT-CLASSIC input.append
  endcase
;

:defer state.alarms  ( key -- )
  case
    KEY.BACK of what's state.menu state! endof
  endcase
;

what's state.dash.locked state!

\ === Main menu ===

variable MAIN-MENU-LEN
4 MAIN-MENU-LEN !

here
\ 1 CELL                \ 3 CELLS
what's state.call     , ," CALL       "
what's state.messages , ," MESSAGES   "
what's state.contacts , ," CONTACTS   "
what's state.alarms   , ," ALARMS     "
MAIN-MENU-LEN @ 4 addr-array MAIN-MENU-ITEMS

: ui.main-menu.show  ( index current? -- )
  cr if ." > " then
  MAIN-MENU-ITEMS array-idx
  1 idx $type
;

: ui.main-menu.go  ( index -- )
  MAIN-MENU-ITEMS array-idx
  0 idx@ state!
;

MAIN-MENU-LEN @ MAIN-MENU s! menu-size
MAIN-MENU-LEN MAIN-MENU s! menu-count
' ui.main-menu.show MAIN-MENU s! menu-show
' ui.main-menu.go MAIN-MENU s! menu-go

\ === Contacts menu ===

: ui.contacts-menu.show  ( index current? -- )
  cr if ." > " then
  CONTACT-LIST contacts.idx contact.show
;

: ui.contacts-menu.go  ( index -- )
  CONTACT-LIST contacts.idx CURRENT-CONTACT !
  what's state.contact state!
;

18 CONTACTS-MENU s! menu-size
CONTACT-LIST CONTACTS-MENU s! menu-count
' ui.contacts-menu.show CONTACTS-MENU s! menu-show
' ui.contacts-menu.go CONTACTS-MENU s! menu-go

\ === Contact menu ===

variable CONTACT-MENU-LEN
5 CONTACT-MENU-LEN !

here
," CALL       "
," MESSAGES   "
2 3 addr-array CONTACT-MENU-ITEMS

: ui.contact-menu.show  { index current? -- }
  cr current? if ." > " then
  index ui.contact-menu.field?
  ?dup if
    what's CURRENT-STATE
    what's state.contact.edit =
    current? and
    if drop CONTACT-EDIT-INPUT input.count type
    else $type then
  else
    index CONTACT-MENU-ITEMS array-idx $type
  then
;

: ui.contact-menu.go  ( index -- )
  dup ui.contact-menu.field?
  if
    drop what's state.contact.edit state!
    CONTACT-EDIT-INPUT input.reset
  else
    case
      CONTACT-MENU-CALL of
        what's state.calling state! endof
      CONTACT-MENU-MESSAGES of
        what's state.discussion state! endof
    endcase
  then
;

5 CONTACT-MENU s! menu-size
CONTACT-MENU-LEN CONTACT-MENU s! menu-count
' ui.contact-menu.show CONTACT-MENU s! menu-show
' ui.contact-menu.go CONTACT-MENU s! menu-go

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

: crs 0 do cr loop ;

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
  12 crs
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
15 cs-array MENU-LABELS

here
what's state.dash.locked       , 1  , 0  ,
what's state.dash.unlock       , 4  , 0  ,
what's state.dash.unlock.write , 6  , 1  ,
what's state.dash              , 2  , 3  ,
what's state.menu              , 4  , 5  ,
what's state.call              , 4  , 0  ,
what's state.call.write        , 6  , 7  ,
what's state.calling           , 4  , 8  ,
what's state.calling.loud      , 4  , 9  ,
what's state.messages          , 4  , 5  ,
what's state.discussion        , 4  , 10 ,
what's state.discussion.write  , 6  , 11 ,
what's state.contacts          , 4  , 5  ,
what's state.contact           , 4  , 12 ,
what's state.contact.edit      , 13 , 14 ,
what's state.alarms            , 4  , 12 ,
what's state.alarms.edit       , 13 , 14 ,
17 3 addr-array MENU-STATES

: ui.menubar  ( pos-x pos-y -- )
  COLOR-PRIMARY -rot
  SCREEN-TILE-W 1 draw.fill-tiles

  what's CURRENT-STATE MENU-STATES array-find throw
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

: ui.unlock  ( -- )
  PASSWORD-INPUT input.count cr type
  17 crs
;

: ui.menu
  MAIN-MENU menu.show
  14 crs
;

: ui.call  ( -- )
  NUMBER-INPUT input.count cr type
  17 crs
;

: ui.calling  ( -- )
  cr ." Calling "
  NUMBER-INPUT input.count type
  17 crs
;

: ui.messages  ( -- )
  cr ." No messages"
  17 crs
;

: ui.contacts  ( -- )
  CONTACTS-MENU menu.show
;

: ui.contact
  CONTACT-MENU menu.show
  13 crs
;

: ui.alarms  ( -- )
  cr ." No alarms"
  17 crs
;

: ui.render  ( -- )
  0 0 ui.status-bar
  what's CURRENT-STATE
  case
    what's state.dash.locked of ui.dash endof
    what's state.dash.unlock of ui.unlock endof
    what's state.dash.unlock.write of ui.unlock endof
    what's state.dash of ui.dash endof
    what's state.menu of ui.menu endof
    what's state.call of ui.call endof
    what's state.call.write of ui.call endof
    what's state.calling of ui.calling endof
    what's state.messages of ui.messages endof
    what's state.contacts of ui.contacts endof
    what's state.contact of ui.contact endof
    what's state.contact.edit of ui.contact endof
    what's state.alarms of ui.alarms endof
  endcase
  0 19 ui.menubar
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

