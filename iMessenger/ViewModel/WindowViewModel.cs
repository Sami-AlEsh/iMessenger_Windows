using System.Windows;

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

        #endregion

        #region Public Properties

        public int ResizeBorder{ get; set; } = 6;

        /// <summary>
        /// The Size of the resize border around the window , taking into account   the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

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
        #endregion

        #region Constractor

        /// <summary>
        /// Default Constractor
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            //Listen out for window resizing
            mWindow.StateChanged += (sender , e) =>{
                //Fire of events for all properties that are affected by a resize
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };
        }

        #endregion
    }
}
