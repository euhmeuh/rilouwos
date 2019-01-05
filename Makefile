DICNAME = rilouwos.dic
PFORTHDIR = ./pforth
PFORTH = $(PFORTHDIR)/pforth -d$(PFORTHDIR)/pforth.dic

.PHONY: all test clean %.fs

$(DICNAME): %.fs
	$(PFORTH) main.fs

all: $(DICNAME)

test:
	$(PFORTH) -d $(DICNAME)

clean:
	rm -f $(DICNAME)
