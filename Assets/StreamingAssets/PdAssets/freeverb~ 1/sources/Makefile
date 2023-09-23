# Makefile for freeverb~ for Makefile.pdlibbuilder

lib.name = freeverb~

freeverb~.class.sources := freeverb~.c

datafiles = \
freeverb~-help.pd \
freeverb~-meta.pd \
output~.pd \
LICENSE.txt \
README.txt \
README.md \
Jezar_readme.txt \
voice.wav


externalsdir = ..
# include Makefile.pdlibbuilder from parent or current directory 
-include $(externalsdir)/Makefile.pdlibbuilder 

ifndef Makefile.pdlibbuilder 
include Makefile.pdlibbuilder 
endif
