using OSRTT_Launcher.DirectX.System;
using SharpDX;
using SharpDX.DirectInput;
using System;

namespace OSRTT_Launcher.DirectX.Input
{
    public class DInput                 // 212 lines
    {
        // Variables & Properties.
        public KeyboardState _KeyboardState;
        public MouseState _MouseState;

        // Properties
        public DirectInput _DirectInput { get; set; }
        public Keyboard _Keyboard { get; set; }
        public Mouse _Mouse { get; set; }
        public int _ScreenWidth { get; set; }
        public int _ScreenHeight { get; set; }
        public int _MouseX { get; set; }
        public int _MouseY { get; set; }
        public string PressedKeys { get; set; }

        // Methods.
        internal bool Initialize(DSystemConfiguration configuration, IntPtr windowsHandle)
        {
            // Screen the screen size which will be used for positioning the mouse cursor.
            _ScreenWidth = configuration.Width;
            _ScreenHeight = configuration.Height;

            // Initialize the location of the mouse on the screen.
			_MouseX = 0;
			_MouseY = 0;

            // Initialize the main direct input interface.
            _DirectInput = new DirectInput();

            #region Keyboard related Initiailization
            // Initialize the direct interface for the keyboard.
            _Keyboard = new Keyboard(_DirectInput);
            _Keyboard.Properties.BufferSize = 256;

            // Set the cooperative level of the keyboard to not share with other programs.
            // use 'CooperativeLevel.Background' for debugging purpose on Both the Mouse and Keyboard as well as NonExclusive on the keybaord.
            _Keyboard.SetCooperativeLevel(windowsHandle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);

            try
            {
                _Keyboard.Acquire();
            }
            catch (SharpDXException sEx)
            {
                if (sEx.ResultCode.Failure)
                    return false;
            }
            #endregion

            #region Mouse related Initiailization
            // Initialize the direct interface for the mouse.
            _Mouse = new Mouse(_DirectInput);
            _Mouse.Properties.AxisMode = DeviceAxisMode.Relative;

            // Set the cooperative level of the mouse to share with other programs.
            _Mouse.SetCooperativeLevel(windowsHandle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);

            try
            {
                _Mouse.Acquire();
            }
            catch (SharpDXException sEx)
            {
                if (sEx.ResultCode.Failure)
                    return false;
            }
            #endregion

            return true;
        }
        // Added this function in Turoail 13.
        public void Shutdown()
        {
            // Release the mouse.
            _Mouse?.Unacquire();
            _Mouse?.Dispose();
            _Mouse = null;
            // Release the keyboard.
            _Keyboard?.Unacquire();
            _Keyboard?.Dispose();
            _Keyboard = null;
            // Release the main interface to direct input.
            _DirectInput?.Dispose();
            _DirectInput = null;
        }
        public bool Frame()
        {
            PressedKeys = "";
            // Read the current state of the keyboard.
            if (!ReadKeyboard())
                return false;
            //ReadKeyboard();
            //ReadMouse();
            // Read the current state of the mouse.
            if (!ReadMouse())
                return false;

            // Process the changes in the mouse and keyboard.
            //ProcessInput();

            return true;
        }
        private bool ReadKeyboard()
        {
            var resultCode = ResultCode.Ok;
            _KeyboardState = new KeyboardState();

            try
            {
                // Read the keyboard device.
                _Keyboard.Poll();
                _Keyboard.GetCurrentState(ref _KeyboardState);
                /*foreach (Key k in _KeyboardState.PressedKeys)
                {
                    // Pressed Keys should be reset per frame in theory. In practice this is better.
                    // This will only output a single key (i.e no rollover support.)
                    PressedKeys = k.ToString();
                    Console.WriteLine(k);
                }*/
            }
            catch (SharpDX.SharpDXException ex)
            {
                resultCode = ex.Descriptor; // ex.ResultCode;
                return false;
            }
            //catch (Exception)
            //{
            //    return false;
            //}

            // Error usually occurs when mouse pointer is off the window and pressed. Short dircuit re aquisition of the mouse when the Mouse cursor if off the Windows.
            //if (DSystem.IsMouseOffScreen)
            //    return true;

            // If the keyboard lost focus or was not acquired then try to get control back.
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _Keyboard.Acquire();
            else if (resultCode != ResultCode.Ok)
                return false;

            return true;
        }
        private bool ReadMouse()
        {
            var resultCode = ResultCode.Ok;

            _MouseState = new MouseState();
            try
            {
                // Read the mouse device.
                _Mouse.GetCurrentState(ref _MouseState);
                if (_MouseState.Buttons[1])
                {
                    PressedKeys = "RightMouseButton";
                }
                if (_MouseState.Buttons[0])
                {
                    PressedKeys = "LeftMouseButton";
                }
            }
            catch (SharpDX.SharpDXException ex)
            {
                // Error usually occurs when mouse pointer is off the window and pressed. Short dircuit re aquisition of the mouse when the Mouse cursor if off the Windows.
                if (DSystem.IsMouseOffScreen)
                    return true;

                resultCode = ex.Descriptor; // ex.ResultCode;

                if (ex.ResultCode.Failure)
                {
                    if (ex.ResultCode == ResultCode.InputLost || ex.ResultCode == ResultCode.NotAcquired)
                    {
                        // If the mouse lost focus or was not acquired then try to get control back.
                        try
                        {
                            _Mouse.Acquire();
                        }
                        catch (SharpDX.SharpDXException exc)
                        { 
                            
                        }
                    }
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            // If the keyboard lost focus or was not acquired then try to get control back.
            /*if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _Mouse.Acquire();
            else if (resultCode != ResultCode.Ok)
                return false;*/

            return true;
        }
        private void ProcessInput()
        {
            if (_MouseState != null)
            {
                _MouseX += _MouseState.X;
                _MouseY += _MouseState.Y;
            }

            // Ensure the mouse location doesn't exceed the screen width or height.
            if (_MouseX < 0) 
                _MouseX = 0;
            if (_MouseY < 0) 
                _MouseY = 0;

            if (_MouseX > _ScreenWidth) 
                _MouseX = _ScreenWidth;
            if (_MouseY > _ScreenHeight) 
                _MouseY = _ScreenHeight;
        }
        public bool IsEscapePressed()
        {
            // Do a bitwise and on the keyboard state to check if the escape key is currently being pressed.
            return _KeyboardState != null && _KeyboardState.PressedKeys.Contains(Key.Escape);
        }
        public void GetMouseLocation(out int mouseX, out int mouseY)
        {
            mouseX = _MouseX;
            mouseY = _MouseY;
        }

    }
}