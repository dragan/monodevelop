
ADDIN_BUILD = $(top_builddir)/build/AddIns/UnitTesting
ASSEMBLY = $(ADDIN_BUILD)/MonoDevelop.UnitTesting.dll

REFS = -r:System

FILES =  \
	AssemblyInfo.cs \
	UnitTest.cs

all: $(ASSEMBLY) $(ASSEMBLY).mdb

$(ASSEMBLY): $(build_sources)
	mkdir -p $(ADDIN_BUILD)
	$(CSC) $(CSC_FLAGS) $(REFS) $(build_sources) -out:$@ -target:library

$(ASSEMBLY).mdb: $(ASSEMBLY)

assemblydir = $(MD_ADDIN_DIR)/UnitTesting
assembly_DATA = $(ASSEMBLY) $(ASSEMBLY).mdb

CLEANFILES = $(ASSEMBLY) $(ASSEMBLY).mdb
EXTRA_DIST = $(FILES)

include $(top_srcdir)/Makefile.include
