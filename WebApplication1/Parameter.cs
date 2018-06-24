using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class Parameter
    {
        private string color;
        private string dataType;
        private string controlName;
        private string fieldName;

        public Parameter(string dataType, string controlName, string fieldName)
        {
            this.dataType = dataType;
            this.fieldName = fieldName;
            this.controlName = controlName;
        }

        public Parameter(string dataType, string controlName)
        {
            this.dataType = dataType;
            this.fieldName = controlName;
            this.controlName = controlName;
        }

        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; }
        }
    }
}