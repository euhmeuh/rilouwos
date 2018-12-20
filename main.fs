\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === INTRODUCTION ===
\
\ Hello and welcome to RilouwOS source code!
\ RilouwOS is an operating system meant to be used in cheap chinese feature-phones
\ running the MediaTek chip MT626x.
\
\ It should support the basic features a phone can do:
\ - Phone calls
\ - Messaging
\ - Contact list
\ - Alarm clock
\
\ It is written in Forth using the pForth kernel cross-compiled for ARM.
\ See the pForth project at http://www.softsynth.com/pforth
\
\ The bootloader loads the Forth compiler, then everything else is bootstrapped
\ from the Forth environment.
\
\ === TECHNICAL INFORMATION ===
\
\ Major system threads:
\ - screen    Refresh the screen
\ - network   Check connection, new calls and messages
\ - keyboard  Listen to the keyboard and broadcast keys
\ - call      Handle current call
\ - message   Send a message
\ - alarm     Check for scheduled alarms
\
\ User Interface:
\ Dashboard          [Unlock     ] [Lock   Menu]
\ - Menu             [Back     OK]
\   - Calling        [Back   Loud] [Back  Quiet]
\   - Call           [Back    Add]
\   - Messages       [Back     OK]
\     - Discussion   [Back  Write] [Erase  Send]
\   - Contacts       [Back     OK]
\     - Contact      [Back   Edit] [Cancel Save]
\   - Alarms         [Back     OK]
\     - Alarm        [Back   Edit] [Cancel Save]
\
\ Keyboard layout:
\ Back         Up            Ok
\ Call2  Left  Enter  Right  Tone
\ Call1        Down          Hang
\ 1 o_o        2 abc         3 def
\ 4 ghi        5 jkl         6 mno
\ 7 pqrs       8 tuv         9 wxyz
\ #            0 _           *

include utils.fs
include build.fs
include date.fs
include draw.fs
include ui.fs

500000 CODE-SIZE !
300000 HEADERS-SIZE !
c" rilouwos.dic" save-forth