using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcerciseTwo.Utilities
{
    public enum ProductType
    {
        Single,
        Package,
        Other
    }

    public static class Extensitons
    {
        public static DateTime ConvertDateEditToDateTime(DateEdit dateEdit )
        {
            DateTime createdDateObj;
            if (!DateTime.TryParse(dateEdit.EditValue.ToString(), out createdDateObj))
            {
                DateTime.TryParse(dateEdit.EditValue.ToString(), out createdDateObj);
            }
            return createdDateObj;
        }
    }
}
