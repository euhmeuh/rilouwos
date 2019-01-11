\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Contact ===
\ Manipulate contacts

21 constant CONTACT-NAME-LEN
21 constant CONTACT-DESC-LEN
21 constant CONTACT-NUMBER-LEN
$01 constant CONTACT-FLAG-FAVORITE
$02 constant CONTACT-FLAG-DELETED

\ Contact structure
\ 1 cell  flags
\ 6 cells name
\ 6 cells desc
\ 6 cells number
19 cells constant CONTACT-LEN

: contact.favorite? @ CONTACT-FLAG-FAVORITE and ;
: contact.deleted? @ CONTACT-FLAG-DELETED and ;

: contact.name  cell+ ;
: contact.desc  7 cells + ;
: contact.number  13 cells + ;

: contact.show  ( contact -- )
  dup contact.name $type ."  "
  dup contact.desc $type ."  "
  contact.number $type
;

: contact.reset  ( contact -- )
  dup 0 swap ! cell+
  dup 0 swap c! 6 cells +
  dup 0 swap c! 6 cells +
  0 swap c!
;

\ === Contact list ===

: contacts.idx ( idx contact-list -- contact )
  2dup @ >= if abort" Index out of bound" then
  cell+ swap CONTACT-LEN * +
;

: contacts.count ( contact-list -- len ) @ ;

: contacts.new  ( contact-list -- contact )
  dup contacts.count 0 do
    dup i swap contacts.idx
    dup contact.deleted?
    if nip 0 leave else drop then
  loop
  ?dup if \ we didn't find any deleted free space
    ( contact-list -- )
    dup dup contacts.count 1+ swap !
    dup contacts.count 1- swap contacts.idx
  then
  dup contact.reset
;
