using System;

namespace KoiVM.Core.CFG;

[Flags]
public enum BlockFlags
{
	Normal = 0,
	ExitEHLeave = 1,
	ExitEHReturn = 2
}
