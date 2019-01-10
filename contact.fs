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

: contacts.idx ( idx contact-list -- contact )
  2dup @ >= if abort" Index out of bound" then
  cell+ swap CONTACT-LEN * +
;
: contacts.count ( contact-list -- len ) @ ;

: contact.show  ( contact -- )
  cr
  dup contact.name $type ."  "
  dup contact.desc $type ."  "
  contact.number $type
;

