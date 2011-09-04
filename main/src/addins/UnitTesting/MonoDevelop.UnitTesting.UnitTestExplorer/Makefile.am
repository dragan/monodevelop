
ADDIN_BUILD = $(top_builddir)/build/AddIns/UnitTesting
ASSEMBLY = $(ADDIN_BUILD)/MonoDevelop.UnitTesting.UnitTestExplorer.dll

DEPS =  \
	$(top_builddir)/build/bin/MonoDevelop.Core.dll \
	$(top_builddir)/build/bin/MonoDevelop.Ide.dll

REFS =  \
	-r:../../../../build/bin/MonoDevelop.Core.dll \
	-r:../../../../build/bin/MonoDevelop.Ide.dll \
	-r:System

FILES = AssemblyInfo.cs

RES = MonoDevelop.UnitTesting.UnitTestExplorer.addin.xml
	
all: $(ASSEMBLY) $(ASSEMBLY).mdb

$(ASSEMBLY): $(build_sources) $(build_resources) $(DEPS)
	mkdir -p $(ADDIN_BUILD)
	$(CSC) $(CSC_FLAGS) $(REFS) $(build_deps) $(build_resources:%=/resource:%) $(build_sources) -out:$@ -target:library

$(ASSEMBLY).mdb: $(ASSEMBLY)

assemblydir = $(MD_ADDIN_DIR)/UnitTesting
assembly_DATA = $(ASSEMBLY) $(ASSEMBLY).mdb

CLEANFILES = $(ASSEMBLY) $(ASSEMBLY).mdb
EXTRA_DIST = $(FILES) $(RES)

include $(top_srcdir)/Makefile.include
