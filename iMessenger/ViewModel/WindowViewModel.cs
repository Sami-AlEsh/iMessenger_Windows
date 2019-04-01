using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace iMessenger
{

    /// <summary>
    /// The View Model of the custom Flat window
    /// </summary>
    class WindowViewModel : BaseViewModel
    {
        #region Private Member

        /// <summary>
        /// The Window this view Model Controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The Margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The Radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width of the window
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;
        /// <summary>
        /// The smallest Height of the window
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless { get { return (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked); } }

        public int ResizeBorder { get { return Borderless ? 0 : 6; } }

        /// <summary>
        /// The Size of the resize border around the window , taking into account   the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The Size of Inner Content of main window
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// The Margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The Margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }


        /// <summary>
        /// The Radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// The Radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The hieght of the title bar/caption of hte window 
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        /// <summary>
        /// The hieght of the title bar/caption of hte window 
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight+ResizeBorder); } }

        /// <summary>
        /// The Current Page in application
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.login;

        #endregion

        #region Commands

        public ICommand MinimizedCommand { get; set; }
        public ICommand MaximizedCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constractor

        /// <summary>
        /// Default Constractor
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            //Create commands
            MinimizedCommand = new RelayCommand(()=> mWindow.WindowState = WindowState.Minimized);
            MaximizedCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand     = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(()=> SystemCommands.ShowSystemMenu(mWindow , new Point(0,0)) );

            var resize = new WindowResizer(mWindow);

            // Listen out for dock changes
            resize.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(mWindow);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }


        #endregion

    }
}
