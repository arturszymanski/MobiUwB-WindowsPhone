using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Utilities
{
    public class UnitIdWrapper
    {
        private String _unitId;
        public String UnitId
        {
            get { return _unitId; }
            set { _unitId = value; }
        }
    }
}
