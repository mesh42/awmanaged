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
using System.ComponentModel;
using System.Reflection;
using System.Web.UI.WebControls;
using AwManaged.Core.Interfaces;
using AwManaged.LocalServices.WebServer.Attributes;

namespace WebApplicationTest
{
    public partial class _Default : System.Web.UI.Page
    {
        private T GettAttribute<T>(PropertyInfo propertyInfo)
        {
            var att = propertyInfo.GetCustomAttributes(typeof(T), false);
            if (att != null && att.Length == 1)
            {
                return (T) att[0];
            }
            return default(T);
        }

        private void BuildGrid<T>(PropertyInfo t) where T : new()
        {
            var q = Database.Storage.Db.Query();
            q.Constrain(typeof (T));
            var set =q.Execute();
            //set.Size();
            
            //var dg = new DataGrid();
            ////dg.DataSource = container;
            //Page.Form.Controls.Add(dg);
        }

        private void BuildForm<T>(T instance)
        {
            var hidden = new System.Web.UI.WebControls.HiddenField();
            hidden.ID = "__Type";
            hidden.Value = instance.ToString();
            Page.Form.Controls.Add(hidden);
            foreach (var property in instance.GetType().GetProperties())
            {
                var att = property.GetCustomAttributes(typeof(WebFormFieldAttribute), false);
                if (att != null && att.Length == 1)
                {
                    // create control.
                    var castAtt = (WebFormFieldAttribute)att[0];
                    var ctrl = new System.Web.UI.WebControls.TextBox();
                    var label = new System.Web.UI.WebControls.Label();
                    ctrl.ID = property.Name;
                    if (property.GetValue(instance, null) != null)
                        ctrl.Text = property.GetValue(instance, null).ToString();

                    var descriptionAtt = GettAttribute<DescriptionAttribute>(property);
                    if (descriptionAtt != null)
                        label.Text = descriptionAtt.Description;
                    else
                        label.Text = ctrl.ID;
                    Page.Form.Controls.Add(label);
                    Page.Form.Controls.Add(new Literal() { Text = "<br />" });
                    Page.Form.Controls.Add(ctrl);

                    if (castAtt.IsRequired)
                        Page.Form.Controls.Add(new RequiredFieldValidator()
                                                   {
                                                       ID = "reqValidate" + property.Name,
                                                       Text = "*",
                                                       Display = ValidatorDisplay.Static,
                                                       ControlToValidate = ctrl.ID
                                                   });
                    
                    if (!string.IsNullOrEmpty(castAtt.RegexPattern))
                    {
                        var validator = new System.Web.UI.WebControls.RegularExpressionValidator();
                        validator.ID = "validate" + property.Name;
                        validator.Text = "*";
                        validator.Display = ValidatorDisplay.Static;
                        validator.ControlToValidate = ctrl.ID;
                        validator.ValidationExpression = castAtt.RegexPattern;
                        Page.Form.Controls.Add(validator);
                    }
                    Page.Form.Controls.Add(new Literal() { Text = "<br />" });
                }
            }
            var submit = new System.Web.UI.WebControls.Button();
            submit.Text = "Submit";
            submit.UseSubmitBehavior = true;
            submit.Click += new EventHandler(submit_Click);
            Page.Form.Controls.Add(submit);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BuildForm(new DataTestClass());
        }

        void submit_Click(object sender, EventArgs e)
        {
            Type t = Type.GetType(Request.Form["__Type"]);
            var instance = Activator.CreateInstance(t);
            foreach (var property in instance.GetType().GetProperties())
            {
                var att = property.GetCustomAttributes(typeof (WebFormFieldAttribute), false);
                if (att != null && att.Length == 1)
                {
                    // create control.
                    var castAtt = (WebFormFieldAttribute) att[0];
                    var value = Request.Form[property.Name];
                    if (property.PropertyType == typeof (string))
                    {
                        property.SetValue(instance, value, null);
                    }
                }
            }

            using (var db = Database.Storage.Clone())
            {
                ((IPersist) instance).Persist(db);
                db.Commit();
            }

            foreach (var property in instance.GetType().GetProperties())
            {
                var att = property.GetCustomAttributes(typeof (WebFormFieldAttribute), false);
                if (att != null && att.Length == 1)
                {
                    ((TextBox) Page.Form.FindControl(property.Name)).Text = string.Empty;
                }

            }
        }
    }
}
