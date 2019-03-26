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

        #endregion

        #region Public Properties

        public string Test { get; set; } = "My String";

        #endregion

        #region Constractor

        /// <summary>
        /// Default Constractor
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;
        }

        #endregion
    }
}
