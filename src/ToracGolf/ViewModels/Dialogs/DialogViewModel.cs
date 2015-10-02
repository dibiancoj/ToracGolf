using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Dialogs
{

    public class DialogViewModel
    {

        #region Constructor

        public DialogViewModel(string dialogHtmlId, string dialogTitle, string dialogBody, IEnumerable<DialogButton> buttons)
        {
            DialogHtmlId = dialogHtmlId;
            DialogTitle = dialogTitle;
            DialogBody = dialogBody;
            DialogButtons = buttons;
        }

        #endregion

        #region Properties

        public string DialogHtmlId { get; }

        public string DialogTitle { get; }

        public string DialogBody { get; }

        public IEnumerable<DialogButton> DialogButtons { get; }

        #endregion

    }

    public class DialogButton
    {
        #region Constructor

        public DialogButton(string buttonText, string classAttribute, bool isCloseButton)
        {
            ButtonText = buttonText;
            ClassAttribute = classAttribute;
            IsCloseButton = isCloseButton;
        }

        #endregion

        #region Properties

        public string ButtonText { get; }

        public string ClassAttribute { get; }

        public bool IsCloseButton { get; }

        #endregion

    }

}
