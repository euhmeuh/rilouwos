\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

: ok ."  OK" cr ;
: fail ."  FAIL" cr abort ;
: should.be.true  if ok else fail then ;
: should.be.false  not should.be.true ;
: should.be.zero  should.be.false ;
: should.be.0<  0< should.be.true ;
: should.be.0>  0> should.be.true ;

input MY-INPUT
4 MY-INPUT input.allot

." NUM mode should append numbers"
'1' MY-INPUT INPUT-NUM input.append
'2' MY-INPUT INPUT-NUM input.append
'3' MY-INPUT INPUT-NUM input.append
'4' MY-INPUT INPUT-NUM input.append
'5' MY-INPUT INPUT-NUM input.append
MY-INPUT input.count s" 1234" compare should.be.zero
MY-INPUT input.reset

." CLASSIC mode should append letters"
'4' MY-INPUT INPUT-CLASSIC input.append
'4' MY-INPUT INPUT-CLASSIC input.append
'3' MY-INPUT INPUT-CLASSIC input.append
'3' MY-INPUT INPUT-CLASSIC input.append
'9' MY-INPUT INPUT-CLASSIC input.append
'9' MY-INPUT INPUT-CLASSIC input.append
'9' MY-INPUT INPUT-CLASSIC input.append
MY-INPUT input.count s" hey" compare should.be.zero
MY-INPUT input.reset

." CLASSIC mode should rotate through letters"
'2' MY-INPUT INPUT-CLASSIC input.append
MY-INPUT input.count s" a" compare
'2' MY-INPUT INPUT-CLASSIC input.append
MY-INPUT input.count s" b" compare or
'2' MY-INPUT INPUT-CLASSIC input.append
MY-INPUT input.count s" c" compare or
'2' MY-INPUT INPUT-CLASSIC input.append
MY-INPUT input.count s" 2" compare or
'2' MY-INPUT INPUT-CLASSIC input.append
MY-INPUT input.count s" a" compare or
should.be.zero
MY-INPUT input.reset

