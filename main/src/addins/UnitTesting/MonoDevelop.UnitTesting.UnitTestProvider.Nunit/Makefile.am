
ADDIN_BUILD = $(top_builddir)/build/AddIns/UnitTesting
ASSEMBLY = $(ADDIN_BUILD)/MonoDevelop.UnitTesting.UnitTestProvider.Nunit.dll

DEPS =  \
	$(top_builddir)/build/bin/MonoDevelop.Core.dll \
	$(top_builddir)/build/bin/MonoDevelop.Ide.dll \
	$(top_builddir)/build/AddIns/UnitTesting/MonoDevelop.UnitTesting.UnitTestExplorer.dll

REFS =  \
	-r:../../../../build/AddIns/UnitTesting/MonoDevelop.UnitTesting.dll \
	-r:../../../../build/AddIns/UnitTesting/MonoDevelop.UnitTesting.UnitTestExplorer.dll \
	-r:../../../../build/bin/MonoDevelop.Core.dll \
	-r:../../../../build/bin/MonoDevelop.Ide.dll \
	-r:../lib/nunit-latest/nunit.core.dll \
	-r:../lib/nunit-latest/nunit.core.interfaces.dll \
	-r:../lib/nunit-latest/nunit.util.dll \
	-r:System

FILES =  \
	AssemblyInfo.cs \
	NunitUnitTestProvider.cs

RES = MonoDevelop.UnitTesting.UnitTestProvider.Nunit.addin.xml

NUNIT_FILES = \
	nunit.core.dll \
	nunit.core.interfaces.dll \
	nunit.util.dll

SRC_NUNIT_FILES = $(addprefix ../lib/nunit-latest/, $(NUNIT_FILES))
BUILD_NUNIT_FILES = $(addprefix $(ADDIN_BUILD)/, $(NUNIT_FILES))

all: $(ASSEMBLY) $(ASSEMBLY).mdb $(BUILD_NUNIT_FILES)

$(ADDIN_BUILD)/%: ../lib/nunit-latest/%
	mkdir -p $(ADDIN_BUILD)
	cp $< $@

$(ASSEMBLY): $(build_sources) $(build_resources) $(DEPS)
	mkdir -p $(ADDIN_BUILD)
	$(CSC) $(CSC_FLAGS) $(REFS) $(build_deps) $(build_resources:%=/resource:%) $(build_sources) -out:$@ -target:library

$(ASSEMBLY).mdb: $(ASSEMBLY)

assemblydir = $(MD_ADDIN_DIR)/UnitTesting
assembly_DATA = $(ASSEMBLY) $(ASSEMBLY).mdb $(SRC_NUNIT_FILES)

CLEANFILES = $(ASSEMBLY) $(ASSEMBLY).mdb $(BUILD_NUNIT_FILES)
EXTRA_DIST = $(FILES) $(RES) $(SRC_NUNIT_FILES)

include $(top_srcdir)/Makefile.include
