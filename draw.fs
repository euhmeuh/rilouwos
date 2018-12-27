\ RilouwOS 0.1.0
\ Copyright (c) 2018 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

\ === Draw ===
\ Rendering primitives

: draw.cursor!  ( x y -- ) 2drop ;

: draw.text  ( str len -- ) type ;

: draw.fill-tiles  ( color x y w h -- ) drop 2drop 2drop ;

