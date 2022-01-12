using System;
using System.Runtime.InteropServices;

public static class WndProcHandler
{
#if !UNITY_EDITOR
    private const uint WM_INITMENUPOPUP = 0x0117;
    private const uint  WM_CANCELMODE = 0x001F;

	// SetWindowLongPtr argument : Sets a new address for the window procedure.
	private const int GWLP_WNDPROC = -4;

	private static bool enabled;
	private static HandleRef hMainWindow;
	private static IntPtr unityWndProcHandler;
	private static IntPtr customWndProcHandler;
	private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	// Instance of delegate
	private static WndProcDelegate procDelegate;

#endif


	public static void Enable(bool bEnable)
	{
#if !UNITY_EDITOR
		if(enabled == false) 
		{
			hMainWindow = new HandleRef(null, GetActiveWindow());
			procDelegate = WndProc;
			customWndProcHandler = Marshal.GetFunctionPointerForDelegate(procDelegate);
			unityWndProcHandler = SetWindowLongPtr(hMainWindow, GWLP_WNDPROC, customWndProcHandler);
			enabled = true;
		}
		else
		{
			SetWindowLongPtr(hMainWindow, GWLP_WNDPROC, unityWndProcHandler);
			hMainWindow = new HandleRef(null, IntPtr.Zero);
			unityWndProcHandler = IntPtr.Zero;
			customWndProcHandler = IntPtr.Zero;
			procDelegate = null;
			enabled = false;
		}
#endif
	}

#if !UNITY_EDITOR
	private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam){
		
		if (msg != WM_INITMENUPOPUP)
			 return DefWindowProc(hWnd, msg, wParam, lParam);
		else 
			 return DefWindowProc(hWnd, WM_CANCELMODE, wParam, lParam);  
	}

	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();

	[DllImport("user32.dll", EntryPoint = "CallWindowProcA")]
	private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint wMsg, IntPtr wParam,
		IntPtr lParam);

	[DllImport("user32.dll", EntryPoint = "DefWindowProcA")]
	private static extern IntPtr DefWindowProc(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

	private static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong){
		if (IntPtr.Size == 8) return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
	}

	[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
	private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
	private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);
#endif
}