#region

using System;

#endregion

namespace MobiUwB.Controls.RoundButtons
{
    public class RoundButtonModel
    {
        private String _text;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private String _imageText;

        public String ImageText
        {
            get { return _imageText; }
            set { _imageText = value; }
        }

        private Object _model;

        public Object Model
        {
            get { return _model; }
            set { _model = value; }
        }


        public RoundButtonModel(String text, String imageText, Object model)
        {
            _text = text;
            _imageText = imageText;
            _model = model;
        }
    }
}
