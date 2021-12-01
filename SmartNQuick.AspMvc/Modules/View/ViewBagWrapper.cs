﻿//@BaseCode
//MdStart

using CommonBase.Extensions;
using SmartNQuick.AspMvc.Models;
using SmartNQuick.AspMvc.Models.Modules.Common;
using SmartNQuick.AspMvc.Models.Modules.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartNQuick.AspMvc.Modules.View
{
    public class ViewBagWrapper
    {
        private dynamic ViewBag { get; set; }
        public ViewBagWrapper(dynamic viewBag)
        {
            ViewBag = viewBag;
        }

        public Type ViewType
        {
            get => ViewBag.ViewType as Type;
            set => ViewBag.ViewType = value;
        }
        public Type ModelType
        {
            get => ViewBag.ModelType as Type;
            set => ViewBag.ModelType = value;
        }
        public bool HasPager
        {
            get => ViewBag.HasPager != null ? (bool)ViewBag.HasPager : true;
            set => ViewBag.HasPager = value;
        }
        public bool HasFilter
        {
            get => ViewBag.HasFilter != null ? (bool)ViewBag.HasFilter : true;
            set => ViewBag.HasFilter = value;
        }
        public ModelCategory ModelCategory
        {
            get
            {
                var result = ModelCategory.Single;

                if (ViewBag.ModelCategory != null)
                {
                    result = ViewBag.ModelCategory;
                }
                return result;
            }
            set => ViewBag.ModelCategory = value;
        }
        public EditMode EditMode
        {
            get
            {
                var result = EditMode.Insert | EditMode.Edit | EditMode.Delete;

                if (ViewBag.EditMode != null)
                {
                    result = ViewBag.EditMode;
                }
                return result;
            }
            set => ViewBag.EditMode = value;
        }
        public CommandMode CommandMode
        {
            get
            {
                var result = CommandMode.Create | CommandMode.Edit | CommandMode.Delete | CommandMode.Details | CommandMode.CreateDetail | CommandMode.EditDetail | CommandMode.DeleteDetail;

                if (ViewBag.CommandMode != null)
                {
                    result = ViewBag.CommandMode;
                }
                return result;
            }
            set => ViewBag.CommandMode = value;
        }
        public object ParentModel
        {
            get => ViewBag.ParentModel as object;
            set => ViewBag.ParentModel = value;
        }

        public string Title => Translate(Controller);
        public string Controller
        {
            get => ViewBag.Controller as string;
            set => ViewBag.Controller = value;
        }
        public string Action
        {
            get => ViewBag.Action as string;
            set => ViewBag.Action = value;
        }
        public string ItemPrefix
        {
            get => ViewBag.ItemPrefix as string;
            set => ViewBag.ItemPrefix = value;
        }
        public string ViewTypeName => ViewType?.FullName;
        public ViewModelCreator ViewModelCreator
        {
            get => ViewBag.ViewModelCreator as ViewModelCreator;
            set => ViewBag.ViewModelCreator = value;
        }

        public int Index
        {
            get => ViewBag.Index != null ? (int)ViewBag.Index : 0;
            set => ViewBag.Index = value;
        }
        public bool Handled
        {
            get => ViewBag.Handled != null ? (bool)ViewBag.Handled : false;
            set => ViewBag.Handled = value;
        }
        public List<string> HiddenNames
        {
            get
            {
                if (ViewBag.HiddenNames is not List<string> result)
                {
                    ViewBag.HiddenNames = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> IgnoreNames
        {
            get
            {
                if (ViewBag.IgnoreNames is not List<string> result)
                {
                    ViewBag.IgnoreNames = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> DisplayNames
        {
            get
            {
                if (ViewBag.DisplayNames is not List<string> result)
                {
                    ViewBag.DisplayNames = result = new List<string>();
                }
                return result;
            }
        }
        public PropertyInfo DisplayProperty
        {
            get => ViewBag.DisplayProperty as PropertyInfo;
            set => ViewBag.DisplayProperty = value;
        }
        public Dictionary<string, string> MappingNames
        {
            get
            {
                if (ViewBag.MappingNames is not Dictionary<string, string> result)
                {
                    ViewBag.MappingNames = result = new Dictionary<string, string>();
                }
                return result;
            }
        }
        public Dictionary<string, PropertyInfo> MappingProperties
        {
            get
            {
                if (ViewBag.MappingProperties is not Dictionary<string, PropertyInfo> result)
                {
                    ViewBag.MappingProperties = result = new Dictionary<string, PropertyInfo>();
                }
                return result;
            }
        }

        public string GetMapping(string key)
        {
            if (MappingNames.TryGetValue(key, out var result) == false)
            {
                result = key;
            }
            return result;
        }
        public void AddMapping(string key, string value)
        {
            if (MappingNames.ContainsKey(key) == false)
            {
               MappingNames.Add(key, value);
            }
        }
        public void AddIgnoreHidden(string name)
        {
            if (IgnoreNames.Contains(name) == false)
                IgnoreNames.Add(name);

            if (HiddenNames.Contains(name) == false)
                HiddenNames.Add(name);
        }

        public bool GetMappingProperty(string key, out PropertyInfo propertyInfo)
        {
            return MappingProperties.TryGetValue(key, out propertyInfo);
        }
        public void AddMappingProperty(string key, PropertyInfo propertyInfo)
        {
            if (MappingProperties.ContainsKey(key) == false)
            {
                MappingProperties.Add(key, propertyInfo);
            }
        }

        public Func<string, string> Translate
        {
            get
            {
                return ViewBag.Translate is Func<string, string> result ? result : s => s;
            }
            set => ViewBag.Translate = value;
        }
        public Func<string, string> TranslateFor => text => Translate($"{Controller}.{text}");

        public IndexViewModel CreateIndexViewModel(IEnumerable<IdentityModel> models)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateIndexViewModel(this, models)
                                            : new ViewModelCreator().CreateIndexViewModel(this, models);
        }

        public DisplayViewModel CreateDisplayViewModel(IdentityModel model)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateDisplayViewModel(this, model)
                                            : new ViewModelCreator().CreateDisplayViewModel(this, model);
        }
        public EditViewModel CreateEditViewModel(IdentityModel model)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateEditViewModel(this, model)
                                            : new ViewModelCreator().CreateEditViewModel(this, model);
        }
    }
}
//MdEnd