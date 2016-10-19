using Caliburn.Micro;

namespace Server.ViewModels
{
    public class DialogViewModel : Screen
    {


        #region Methods

        public void Ok()
        {
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        #endregion

    }
}