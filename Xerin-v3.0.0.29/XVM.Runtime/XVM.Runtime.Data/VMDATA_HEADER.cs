using XVM.Runtime.Dynamic;

namespace XVM.Runtime.Data;

internal struct VMDATA_HEADER
{
	public byte REG_R0;

	public byte REG_R1;

	public byte REG_R2;

	public byte REG_R3;

	public byte REG_R4;

	public byte REG_R5;

	public byte REG_R6;

	public byte REG_R7;

	public byte REG_BP;

	public byte REG_SP;

	public byte REG_IP;

	public byte REG_FL;

	public byte REG_K1;

	public byte FL_OVERFLOW;

	public byte FL_CARRY;

	public byte FL_ZERO;

	public byte FL_SIGN;

	public byte FL_UNSIGNED;

	public byte OP_NOP;

	public byte OP_LIND_PTR;

	public byte OP_LIND_OBJECT;

	public byte OP_LIND_BYTE;

	public byte OP_LIND_WORD;

	public byte OP_LIND_DWORD;

	public byte OP_LIND_QWORD;

	public byte OP_SIND_PTR;

	public byte OP_SIND_OBJECT;

	public byte OP_SIND_BYTE;

	public byte OP_SIND_WORD;

	public byte OP_SIND_DWORD;

	public byte OP_SIND_QWORD;

	public byte OP_POP;

	public byte OP_PUSHR_OBJECT;

	public byte OP_PUSHR_BYTE;

	public byte OP_PUSHR_WORD;

	public byte OP_PUSHR_DWORD;

	public byte OP_PUSHR_QWORD;

	public byte OP_PUSHI_DWORD;

	public byte OP_PUSHI_QWORD;

	public byte OP_SX_BYTE;

	public byte OP_SX_WORD;

	public byte OP_SX_DWORD;

	public byte OP_CALL;

	public byte OP_RET;

	public byte OP_NOR_DWORD;

	public byte OP_NOR_QWORD;

	public byte OP_CMP;

	public byte OP_CMP_DWORD;

	public byte OP_CMP_QWORD;

	public byte OP_CMP_R32;

	public byte OP_CMP_R64;

	public byte OP_JZ;

	public byte OP_JNZ;

	public byte OP_JMP;

	public byte OP_SWT;

	public byte OP_ADD_DWORD;

	public byte OP_ADD_QWORD;

	public byte OP_ADD_R32;

	public byte OP_ADD_R64;

	public byte OP_SUB_R32;

	public byte OP_SUB_R64;

	public byte OP_MUL_DWORD;

	public byte OP_MUL_QWORD;

	public byte OP_MUL_R32;

	public byte OP_MUL_R64;

	public byte OP_DIV_DWORD;

	public byte OP_DIV_QWORD;

	public byte OP_DIV_R32;

	public byte OP_DIV_R64;

	public byte OP_REM_DWORD;

	public byte OP_REM_QWORD;

	public byte OP_REM_R32;

	public byte OP_REM_R64;

	public byte OP_SHR_DWORD;

	public byte OP_SHR_QWORD;

	public byte OP_SHL_DWORD;

	public byte OP_SHL_QWORD;

	public byte OP_FCONV_R32_R64;

	public byte OP_FCONV_R64_R32;

	public byte OP_FCONV_R32;

	public byte OP_FCONV_R64;

	public byte OP_ICONV_PTR;

	public byte OP_ICONV_R64;

	public byte OP_VCALL;

	public byte OP_TRY;

	public byte OP_LEAVE;

	public byte VCALL_EXIT;

	public byte VCALL_BREAK;

	public byte VCALL_ECALL;

	public byte VCALL_CAST;

	public byte VCALL_CKFINITE;

	public byte VCALL_CKOVERFLOW;

	public byte VCALL_RANGECHK;

	public byte VCALL_INITOBJ;

	public byte VCALL_LDFLD;

	public byte VCALL_LDFTN;

	public byte VCALL_TOKEN;

	public byte VCALL_THROW;

	public byte VCALL_SIZEOF;

	public byte VCALL_STFLD;

	public byte VCALL_BOX;

	public byte VCALL_UNBOX;

	public byte VCALL_LOCALLOC;

	public byte ECALL_CALL;

	public byte ECALL_CALLVIRT;

	public byte ECALL_NEWOBJ;

	public byte ECALL_CALLVIRT_CONSTRAINED;

	public byte EH_CATCH;

	public byte EH_FILTER;

	public byte EH_FAULT;

	public byte EH_FINALLY;

	public Constants ToConstants()
	{
		Constants constants = new Constants();
		constants.REG_R0 = REG_R0;
		constants.REG_R1 = REG_R1;
		constants.REG_R2 = REG_R2;
		constants.REG_R3 = REG_R3;
		constants.REG_R4 = REG_R4;
		constants.REG_R5 = REG_R5;
		constants.REG_R6 = REG_R6;
		constants.REG_R7 = REG_R7;
		constants.REG_BP = REG_BP;
		constants.REG_SP = REG_SP;
		constants.REG_IP = REG_IP;
		constants.REG_FL = REG_FL;
		constants.REG_K1 = REG_K1;
		constants.FL_OVERFLOW = FL_OVERFLOW;
		constants.FL_CARRY = FL_CARRY;
		constants.FL_ZERO = FL_ZERO;
		constants.FL_SIGN = FL_SIGN;
		constants.FL_UNSIGNED = FL_UNSIGNED;
		constants.OP_NOP = OP_NOP;
		constants.OP_LIND_PTR = OP_LIND_PTR;
		constants.OP_LIND_OBJECT = OP_LIND_OBJECT;
		constants.OP_LIND_BYTE = OP_LIND_BYTE;
		constants.OP_LIND_WORD = OP_LIND_WORD;
		constants.OP_LIND_DWORD = OP_LIND_DWORD;
		constants.OP_LIND_QWORD = OP_LIND_QWORD;
		constants.OP_SIND_PTR = OP_SIND_PTR;
		constants.OP_SIND_OBJECT = OP_SIND_OBJECT;
		constants.OP_SIND_BYTE = OP_SIND_BYTE;
		constants.OP_SIND_WORD = OP_SIND_WORD;
		constants.OP_SIND_DWORD = OP_SIND_DWORD;
		constants.OP_SIND_QWORD = OP_SIND_QWORD;
		constants.OP_POP = OP_POP;
		constants.OP_PUSHR_OBJECT = OP_PUSHR_OBJECT;
		constants.OP_PUSHR_BYTE = OP_PUSHR_BYTE;
		constants.OP_PUSHR_WORD = OP_PUSHR_WORD;
		constants.OP_PUSHR_DWORD = OP_PUSHR_DWORD;
		constants.OP_PUSHR_QWORD = OP_PUSHR_QWORD;
		constants.OP_PUSHI_DWORD = OP_PUSHI_DWORD;
		constants.OP_PUSHI_QWORD = OP_PUSHI_QWORD;
		constants.OP_SX_BYTE = OP_SX_BYTE;
		constants.OP_SX_WORD = OP_SX_WORD;
		constants.OP_SX_DWORD = OP_SX_DWORD;
		constants.OP_CALL = OP_CALL;
		constants.OP_RET = OP_RET;
		constants.OP_NOR_DWORD = OP_NOR_DWORD;
		constants.OP_NOR_QWORD = OP_NOR_QWORD;
		constants.OP_CMP = OP_CMP;
		constants.OP_CMP_DWORD = OP_CMP_DWORD;
		constants.OP_CMP_QWORD = OP_CMP_QWORD;
		constants.OP_CMP_R32 = OP_CMP_R32;
		constants.OP_CMP_R64 = OP_CMP_R64;
		constants.OP_JZ = OP_JZ;
		constants.OP_JNZ = OP_JNZ;
		constants.OP_JMP = OP_JMP;
		constants.OP_SWT = OP_SWT;
		constants.OP_ADD_DWORD = OP_ADD_DWORD;
		constants.OP_ADD_QWORD = OP_ADD_QWORD;
		constants.OP_ADD_R32 = OP_ADD_R32;
		constants.OP_ADD_R64 = OP_ADD_R64;
		constants.OP_SUB_R32 = OP_SUB_R32;
		constants.OP_SUB_R64 = OP_SUB_R64;
		constants.OP_MUL_DWORD = OP_MUL_DWORD;
		constants.OP_MUL_QWORD = OP_MUL_QWORD;
		constants.OP_MUL_R32 = OP_MUL_R32;
		constants.OP_MUL_R64 = OP_MUL_R64;
		constants.OP_DIV_DWORD = OP_DIV_DWORD;
		constants.OP_DIV_QWORD = OP_DIV_QWORD;
		constants.OP_DIV_R32 = OP_DIV_R32;
		constants.OP_DIV_R64 = OP_DIV_R64;
		constants.OP_REM_DWORD = OP_REM_DWORD;
		constants.OP_REM_QWORD = OP_REM_QWORD;
		constants.OP_REM_R32 = OP_REM_R32;
		constants.OP_REM_R64 = OP_REM_R64;
		constants.OP_SHR_DWORD = OP_SHR_DWORD;
		constants.OP_SHR_QWORD = OP_SHR_QWORD;
		constants.OP_SHL_DWORD = OP_SHL_DWORD;
		constants.OP_SHL_QWORD = OP_SHL_QWORD;
		constants.OP_FCONV_R32_R64 = OP_FCONV_R32_R64;
		constants.OP_FCONV_R64_R32 = OP_FCONV_R64_R32;
		constants.OP_FCONV_R32 = OP_FCONV_R32;
		constants.OP_FCONV_R64 = OP_FCONV_R64;
		constants.OP_ICONV_PTR = OP_ICONV_PTR;
		constants.OP_ICONV_R64 = OP_ICONV_R64;
		constants.OP_VCALL = OP_VCALL;
		constants.OP_TRY = OP_TRY;
		constants.OP_LEAVE = OP_LEAVE;
		constants.VCALL_EXIT = VCALL_EXIT;
		constants.VCALL_BREAK = VCALL_BREAK;
		constants.VCALL_ECALL = VCALL_ECALL;
		constants.VCALL_CAST = VCALL_CAST;
		constants.VCALL_CKFINITE = VCALL_CKFINITE;
		constants.VCALL_CKOVERFLOW = VCALL_CKOVERFLOW;
		constants.VCALL_RANGECHK = VCALL_RANGECHK;
		constants.VCALL_INITOBJ = VCALL_INITOBJ;
		constants.VCALL_LDFLD = VCALL_LDFLD;
		constants.VCALL_LDFTN = VCALL_LDFTN;
		constants.VCALL_TOKEN = VCALL_TOKEN;
		constants.VCALL_THROW = VCALL_THROW;
		constants.VCALL_SIZEOF = VCALL_SIZEOF;
		constants.VCALL_STFLD = VCALL_STFLD;
		constants.VCALL_BOX = VCALL_BOX;
		constants.VCALL_UNBOX = VCALL_UNBOX;
		constants.VCALL_LOCALLOC = VCALL_LOCALLOC;
		constants.ECALL_CALL = ECALL_CALL;
		constants.ECALL_CALLVIRT = ECALL_CALLVIRT;
		constants.ECALL_NEWOBJ = ECALL_NEWOBJ;
		constants.ECALL_CALLVIRT_CONSTRAINED = ECALL_CALLVIRT_CONSTRAINED;
		constants.EH_CATCH = EH_CATCH;
		constants.EH_FILTER = EH_FILTER;
		constants.EH_FAULT = EH_FAULT;
		constants.EH_FINALLY = EH_FINALLY;
		return constants;
	}
}
