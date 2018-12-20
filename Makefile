DICNAME = rilouwos.dic

.PHONY: all test clean %.fs

$(DICNAME): %.fs
	pforth main.fs

all: $(DICNAME)

test:
	pforth -d $(DICNAME)

clean:
	rm -f $(DICNAME)
