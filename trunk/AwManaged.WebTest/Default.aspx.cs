/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
//using AwManaged.LocalServices.WebServer.Attributes;

namespace AwManaged.WebTest
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var c = new DataTestClass();

            //foreach (var property in c.GetType().GetProperties())
            //{
            //    var att = property.GetCustomAttributes(typeof(WebFormFieldAttribute), false);
            //    if (att !=null && att.Length == 1)
            //    {
            //        // create control.
            //        var castAtt = (WebFormFieldAttribute) att[0];
            //        var ctrl = new System.Web.UI.WebControls.TextBox();
            //        ctrl.ID = property.Name;
            //        Page.Form.Controls.Add(ctrl);
            //        if (!string.IsNullOrEmpty(castAtt.RegexPattern))
            //        {
            //            var validator = new System.Web.UI.WebControls.RegularExpressionValidator();
            //            validator.ID = "validate" + property.Name;
            //            validator.ControlToValidate = ctrl.ID;
            //            validator.ValidationExpression = castAtt.RegexPattern;
            //        }
            //    }
            //}
        }
    }
}
