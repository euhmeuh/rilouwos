\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

: ok ."  OK" cr ;
: fail ."  FAIL" cr abort ;
: should.be.true  if ok else fail then ;
: should.be.false  not should.be.true ;
: should.be.0<  0< should.be.true ;
: should.be.0>  0> should.be.true ;

input MY-INPUT
4 MY-INPUT input.allot

." NUM mode should append numbers"
input.mode.NUM
'1' MY-INPUT input.append
'2' MY-INPUT input.append
'3' MY-INPUT input.append
'4' MY-INPUT input.append
'5' MY-INPUT input.append
MY-INPUT input.count s" 1234" compare
should.be.false
MY-INPUT input.reset

." CLASSIC mode should append letters"
input.mode.CLASSIC
'4' MY-INPUT input.append
'4' MY-INPUT input.append
'3' MY-INPUT input.append
'3' MY-INPUT input.append
'9' MY-INPUT input.append
'9' MY-INPUT input.append
'9' MY-INPUT input.append
MY-INPUT input.count s" hey" compare
should.be.false
MY-INPUT input.reset

